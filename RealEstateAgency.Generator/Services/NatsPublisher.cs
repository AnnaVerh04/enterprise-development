using NATS.Client.Core;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace RealEstateAgency.Generator.Services;

/// <summary>
/// Сервис публикации сообщений в NATS с поддержкой ретраев
/// </summary>
public class NatsPublisher : IAsyncDisposable
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly ILogger<NatsPublisher> _logger;
    private readonly string _connectionString;
    private NatsConnection? _connection;
    private readonly ResiliencePipeline _retryPipeline;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private bool _disposed;

    public NatsPublisher(string connectionString, ILogger<NatsPublisher> logger)
    {
        _connectionString = connectionString;
        _logger = logger;

        _retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 10,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                MaxDelay = TimeSpan.FromMinutes(2),
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "Попытка подключения к NATS #{AttemptNumber} не удалась. " +
                        "Следующая попытка через {Delay}. Ошибка: {Error}",
                        args.AttemptNumber + 1,
                        args.RetryDelay,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    /// <summary>
    /// Подключение к NATS с ретраями
    /// </summary>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            if (_connection != null)
                return;

            await _retryPipeline.ExecuteAsync(async ct =>
            {
                _logger.LogInformation("Подключение к NATS: {ConnectionString}", _connectionString);

                var options = new NatsOpts
                {
                    Url = _connectionString,
                    Name = "RealEstateAgency.Generator",
                    ConnectTimeout = TimeSpan.FromSeconds(10)
                };

                _connection = new NatsConnection(options);
                await _connection.ConnectAsync();

                _logger.LogInformation("Успешное подключение к NATS");
            }, cancellationToken);
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    /// <summary>
    /// Публикация сообщения с гарантией доставки
    /// </summary>
    public async Task PublishAsync<T>(string subject, T data, CancellationToken cancellationToken = default)
    {
        if (_connection == null)
        {
            await ConnectAsync(cancellationToken);
        }

        await _retryPipeline.ExecuteAsync(async ct =>
        {
            if (_connection == null)
                throw new InvalidOperationException("Нет подключения к NATS");

            var jsonData = JsonSerializer.Serialize(data, _jsonOptions);

            await _connection.PublishAsync(subject, jsonData, cancellationToken: ct);

            _logger.LogDebug("Опубликовано сообщение в {Subject}", subject);
        }, cancellationToken);
    }

    /// <summary>
    /// Потоковая публикация последовательности сообщений
    /// </summary>
    public async Task PublishStreamAsync<T>(
        string subject,
        IAsyncEnumerable<T> dataStream,
        CancellationToken cancellationToken = default)
    {
        if (_connection == null)
        {
            await ConnectAsync(cancellationToken);
        }

        await foreach (var data in dataStream.WithCancellation(cancellationToken))
        {
            await PublishAsync(subject, data, cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }

        _connectionLock.Dispose();

        GC.SuppressFinalize(this);
    }
}