using NATS.Client.Core;
using Polly;
using Polly.Retry;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;
using System.Text.Json;

namespace RealEstateAgency.WebApi.Services;

/// <summary>
/// Фоновый сервис для получения данных из NATS и сохранения в БД
/// </summary>
public class NatsSubscriberService : BackgroundService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NatsSubscriberService> _logger;
    private readonly string _connectionString;
    private readonly ResiliencePipeline _retryPipeline;
    private NatsConnection? _connection;

    private const string CounterpartyTopic = "realestate.counterparty.created";
    private const string PropertyTopic = "realestate.property.created";

    public NatsSubscriberService(
        IServiceProvider serviceProvider,
        ILogger<NatsSubscriberService> logger,
        string connectionString)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _connectionString = connectionString;

        _retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = int.MaxValue,
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Запуск NATS Subscriber Service");

        try
        {
            await ConnectWithRetryAsync(stoppingToken);

            if (_connection == null)
            {
                _logger.LogError("Не удалось подключиться к NATS");
                return;
            }

            var counterpartyTask = SubscribeToCounterpartiesAsync(stoppingToken);
            var propertyTask = SubscribeToPropertiesAsync(stoppingToken);

            await Task.WhenAll(counterpartyTask, propertyTask);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("NATS Subscriber Service остановлен");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Критическая ошибка в NATS Subscriber Service");
            throw;
        }
    }

    private async Task ConnectWithRetryAsync(CancellationToken cancellationToken)
    {
        await _retryPipeline.ExecuteAsync(async ct =>
        {
            _logger.LogInformation("Подключение к NATS: {ConnectionString}", _connectionString);

            var options = new NatsOpts
            {
                Url = _connectionString,
                Name = "RealEstateAgency.WebApi",
                ConnectTimeout = TimeSpan.FromSeconds(10)
            };

            _connection = new NatsConnection(options);
            await _connection.ConnectAsync();

            _logger.LogInformation("Успешное подключение к NATS");
        }, cancellationToken);
    }

    private async Task SubscribeToCounterpartiesAsync(CancellationToken cancellationToken)
    {
        if (_connection == null) return;

        _logger.LogInformation("Подписка на топик: {Topic}", CounterpartyTopic);

        await foreach (var msg in _connection.SubscribeAsync<string>(CounterpartyTopic, cancellationToken: cancellationToken))
        {
            try
            {
                if (string.IsNullOrEmpty(msg.Data))
                    continue;

                var dto = JsonSerializer.Deserialize<CreateCounterpartyDto>(msg.Data, _jsonOptions);

                if (dto == null)
                    continue;

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICounterpartyService>();

                var result = await service.CreateAsync(dto);
                _logger.LogDebug("Создан контрагент: {Id} - {FullName}", result.Id, result.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения контрагента");
            }
        }
    }

    private async Task SubscribeToPropertiesAsync(CancellationToken cancellationToken)
    {
        if (_connection == null) return;

        _logger.LogInformation("Подписка на топик: {Topic}", PropertyTopic);

        await foreach (var msg in _connection.SubscribeAsync<string>(PropertyTopic, cancellationToken: cancellationToken))
        {
            try
            {
                if (string.IsNullOrEmpty(msg.Data))
                    continue;

                var dto = JsonSerializer.Deserialize<CreateRealEstatePropertyDto>(msg.Data, _jsonOptions);

                if (dto == null)
                    continue;

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IRealEstatePropertyService>();

                var result = await service.CreateAsync(dto);
                _logger.LogDebug("Создан объект недвижимости: {Id} - {Address}", result.Id, result.Address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения недвижимости");
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Остановка NATS Subscriber Service...");

        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}