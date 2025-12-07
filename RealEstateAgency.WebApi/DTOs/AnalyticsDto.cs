using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.WebApi.DTOs;

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

/// <summary>
/// DTO для топ клиентов по количеству заявок
/// </summary>
public class TopClientDto
{
    /// <summary>
    /// ФИО клиента
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Количество заявок
    /// </summary>
    public int RequestCount { get; set; }
}

/// <summary>
/// DTO для клиента с минимальной суммой заявки
/// </summary>
public class ClientWithMinAmountDto
{
    /// <summary>
    /// ФИО клиента
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Минимальная сумма
    /// </summary>
    public decimal MinAmount { get; set; }
}

/// <summary>
/// DTO результата топ-5 клиентов (покупка и продажа)
/// </summary>
public class Top5ClientsResultDto
{
    /// <summary>
    /// Топ-5 покупателей
    /// </summary>
    public List<TopClientDto> TopPurchaseClients { get; set; } = [];

    /// <summary>
    /// Топ-5 продавцов
    /// </summary>
    public List<TopClientDto> TopSaleClients { get; set; } = [];
}
