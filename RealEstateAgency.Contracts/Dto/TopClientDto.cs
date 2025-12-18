namespace RealEstateAgency.Contracts.Dto;

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
