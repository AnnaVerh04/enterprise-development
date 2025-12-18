namespace RealEstateAgency.Contracts.Dto;

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
