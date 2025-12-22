namespace RealEstateAgency.Domain.Models;

/// <summary>
/// Контрагент агентства недвижимости
/// Физическое лицо, участвующее в сделках с недвижимостью
/// </summary>
public class Counterparty
{
    /// <summary>
    /// Уникальный идентификатор контрагента
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Полное наименование контрагента в формате "Фамилия, имя, отчество"
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Номер паспорта для идентификации личности
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Контактный телефон для связи
    /// </summary>
    public required string PhoneNumber { get; set; }
}
