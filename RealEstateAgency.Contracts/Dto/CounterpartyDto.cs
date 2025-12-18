namespace RealEstateAgency.Contracts.Dto;

/// <summary>
/// DTO для отображения контрагента
/// </summary>
public class CounterpartyDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ФИО контрагента
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Номер паспорта
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Контактный телефон
    /// </summary>
    public required string PhoneNumber { get; set; }
}
