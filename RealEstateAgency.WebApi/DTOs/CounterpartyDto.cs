namespace RealEstateAgency.WebApi.DTOs;

/// <summary>
/// DTO для отображения контрагента
/// </summary>
public class CounterpartyDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public int Id { get; set; }

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

/// <summary>
/// DTO для создания контрагента
/// </summary>
public class CreateCounterpartyDto
{
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

/// <summary>
/// DTO для обновления контрагента
/// </summary>
public class UpdateCounterpartyDto
{
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
