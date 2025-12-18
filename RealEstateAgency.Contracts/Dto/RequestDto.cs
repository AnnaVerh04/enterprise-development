using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Contracts.Dto;

/// <summary>
/// DTO для отображения заявки
/// </summary>
public class RequestDto
{
    /// <summary>
    /// Уникальный идентификатор заявки
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    public Guid CounterpartyId { get; set; }

    /// <summary>
    /// Данные контрагента
    /// </summary>
    public CounterpartyDto? Counterparty { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    public Guid PropertyId { get; set; }

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
