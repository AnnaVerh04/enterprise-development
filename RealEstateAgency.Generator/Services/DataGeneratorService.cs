using Microsoft.Extensions.Options;
using RealEstateAgency.Generator.Generators;

namespace RealEstateAgency.Generator.Services;

/// <summary>
/// Фоновый сервис для потоковой генерации и отправки контрактов в NATS
/// </summary>
public class DataGeneratorService(
    NatsPublisher natsPublisher,
    ILogger<DataGeneratorService> logger,
    IOptions<DataGeneratorSettings> options) : BackgroundService
{
    private readonly ILogger<DataGeneratorService> _logger = logger;
    private readonly DataGeneratorSettings _settings = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Запуск сервиса генерации данных. Размер пакета: {BatchSize}, задержка: {Delay}мс",
            _settings.BatchSize,
            _settings.DelayBetweenBatchesMs);

        try
        {
            await natsPublisher.ConnectAsync(stoppingToken);

            _logger.LogInformation("Начало потоковой генерации контрактов");

            var totalGenerated = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await GenerateAndSendBatchAsync(_settings.BatchSize, stoppingToken);

                    totalGenerated += _settings.BatchSize;
                    _logger.LogInformation(
                        "Сгенерировано и отправлено {BatchSize} контрактов. Всего: {Total}",
                        _settings.BatchSize,
                        totalGenerated);

                    await Task.Delay(_settings.DelayBetweenBatchesMs, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при генерации пакета данных");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Сервис генерации данных остановлен");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Критическая ошибка в сервисе генерации данных");
            throw;
        }
    }

    /// <summary>
    /// Генерация и отправка пакета данных в потоковом режиме
    /// </summary>
    private async Task GenerateAndSendBatchAsync(int count, CancellationToken cancellationToken)
    {
        await foreach (var data in GenerateDataStreamAsync(count).WithCancellation(cancellationToken))
        {
            await natsPublisher.PublishAsync(_settings.CounterpartyTopic, data.Counterparty, cancellationToken);
            _logger.LogDebug("Отправлен контрагент: {FullName}", data.Counterparty.FullName);

            await Task.Delay(_settings.DelayBetweenMessagesMs, cancellationToken);

            await natsPublisher.PublishAsync(_settings.PropertyTopic, data.Property, cancellationToken);
            _logger.LogDebug("Отправлена недвижимость: {Address}", data.Property.Address);

            await Task.Delay(_settings.DelayBetweenMessagesMs, cancellationToken);
        }
    }

    /// <summary>
    /// Асинхронный генератор данных (потоковая генерация)
    /// </summary>
    private static async IAsyncEnumerable<GeneratedDataPackage> GenerateDataStreamAsync(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await Task.Yield();

            yield return ContractGenerator.GenerateDataPackage();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Остановка сервиса генерации данных...");
        await base.StopAsync(cancellationToken);
        await natsPublisher.DisposeAsync();
    }
}