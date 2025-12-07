using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.WebApi.DTOs;

/// <summary>
/// DTO для отображения заявки
/// </summary>
public class RequestDto
{
    /// <summary>
    /// Уникальный идентификатор заявки
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    public int CounterpartyId { get; set; }

    /// <summary>
    /// Данные контрагента
    /// </summary>
    public CounterpartyDto? Counterparty { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    public int PropertyId { get; set; }

    /// <summary>
    /// Данные объекта недвижимости
    /// </summary>
    public RealEstatePropertyDto? Property { get; set; }

    /// <summary>
    /// Тип заявки (покупка/продажа)
    /// </summary>
    public RequestType Type { get; set; }

    /// <summary>
    /// Сумма сделки
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    public DateTime Date { get; set; }
}

/// <summary>
/// DTO для создания заявки
/// </summary>
public class CreateRequestDto
{
    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    public int CounterpartyId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    public int PropertyId { get; set; }

    /// <summary>
    /// Тип заявки (покупка/продажа)
    /// </summary>
    public RequestType Type { get; set; }

    /// <summary>
    /// Сумма сделки
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    public DateTime Date { get; set; }
}

/// <summary>
/// DTO для обновления заявки
/// </summary>
public class UpdateRequestDto
{
    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    public int CounterpartyId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    public int PropertyId { get; set; }

    /// <summary>
    /// Тип заявки (покупка/продажа)
    /// </summary>
    public RequestType Type { get; set; }

    /// <summary>
    /// Сумма сделки
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    public DateTime Date { get; set; }
}
