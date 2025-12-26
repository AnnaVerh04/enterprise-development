using Microsoft.Extensions.Options;

namespace RealEstateAgency.Generator.Services;

/// <summary>
/// Настройки сервиса генерации данных
/// </summary>
public class DataGeneratorSettings
{
    public const string SectionName = "DataGenerator";

    public int BatchSize { get; set; } = 10;
    public int DelayBetweenBatchesMs { get; set; } = 5000;
    public int DelayBetweenMessagesMs { get; set; } = 100;

    public string CounterpartyTopic { get; set; } = "realestate.counterparty.created";
    public string PropertyTopic { get; set; } = "realestate.property.created";
    public string RequestTopic { get; set; } = "realestate.request.created";
}