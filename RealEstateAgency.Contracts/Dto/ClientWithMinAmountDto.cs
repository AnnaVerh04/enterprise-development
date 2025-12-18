namespace RealEstateAgency.Contracts.Dto;

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
