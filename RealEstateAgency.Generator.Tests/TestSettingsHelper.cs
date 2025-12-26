using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealEstateAgency.Generator.Services;

namespace RealEstateAgency.Generator.Tests;

/// <summary>
/// Вспомогательный класс для настроек тестов
/// </summary>
public static class TestSettingsHelper
{
    public const string CounterpartyTopic = "realestate.counterparty.created";
    public const string PropertyTopic = "realestate.property.created";
    public const string RequestTopic = "realestate.request.created";

    /// <summary>
    /// Создает настройки для тестов
    /// </summary>
    public static IOptions<DataGeneratorSettings> CreateTestOptions(
        int batchSize = 10,
        int delayBetweenBatchesMs = 5000,
        int delayBetweenMessagesMs = 100)
    {
        return Options.Create(new DataGeneratorSettings
        {
            BatchSize = batchSize,
            DelayBetweenBatchesMs = delayBetweenBatchesMs,
            DelayBetweenMessagesMs = delayBetweenMessagesMs,
            CounterpartyTopic = CounterpartyTopic,
            PropertyTopic = PropertyTopic,
            RequestTopic = RequestTopic
        });
    }

    /// <summary>
    /// Создает экземпляр DataGeneratorService для тестов
    /// </summary>
    public static DataGeneratorService CreateTestService(
        NatsPublisher publisher,
        ILogger<DataGeneratorService> logger,
        int batchSize = 10,
        int delayBetweenBatchesMs = 5000)
    {
        var options = CreateTestOptions(batchSize, delayBetweenBatchesMs);
        return new DataGeneratorService(publisher, logger, options);
    }
}