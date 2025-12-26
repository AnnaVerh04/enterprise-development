using RealEstateAgency.Contracts.Dto;

namespace RealEstateAgency.Generator.Generators;

/// <summary>
/// Пакет сгенерированных данных для отправки
/// </summary>
public class GeneratedDataPackage
{
    public required CreateCounterpartyDto Counterparty { get; init; }
    public required CreateRealEstatePropertyDto Property { get; init; }
}