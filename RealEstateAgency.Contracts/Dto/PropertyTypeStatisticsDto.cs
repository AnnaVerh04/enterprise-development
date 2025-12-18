using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Contracts.Dto;

/// <summary>
/// DTO для статистики по типу недвижимости
/// </summary>
public class PropertyTypeStatisticsDto
{
    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public PropertyType PropertyType { get; set; }

    /// <summary>
    /// Количество заявок
    /// </summary>
    public int RequestCount { get; set; }
}
