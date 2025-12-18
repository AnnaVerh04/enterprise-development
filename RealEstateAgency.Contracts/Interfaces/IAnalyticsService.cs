using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Contracts.Interfaces;

/// <summary>
/// Интерфейс сервиса аналитики
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    public Task<IEnumerable<string>> GetSellersInPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получить топ-5 клиентов по количеству заявок (покупка и продажа отдельно)
    /// </summary>
    public Task<Top5ClientsResultDto> GetTop5ClientsByRequestCountAsync();

    /// <summary>
    /// Получить статистику заявок по типам недвижимости
    /// </summary>
    public Task<IEnumerable<PropertyTypeStatisticsDto>> GetRequestCountByPropertyTypeAsync();

    /// <summary>
    /// Получить клиентов с заявками минимальной стоимости
    /// </summary>
    public Task<ClientWithMinAmountDto> GetClientsWithMinAmountAsync();

    /// <summary>
    /// Получить клиентов, ищущих определённый тип недвижимости
    /// </summary>
    public Task<IEnumerable<string>> GetClientsSeekingPropertyTypeAsync(PropertyType propertyType);
}
