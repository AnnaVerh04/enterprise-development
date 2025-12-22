using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Models;

/// <summary>
/// Заявка на совершение сделки с недвижимостью
/// Это соглашение между контрагентом и агентством.
/// </summary>
public class Request
{
    /// <summary>
    /// Уникальный идентификатор приложения
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор контрагента (внешний ключ)
    /// </summary>
    public Guid CounterpartyId { get; set; }

    /// <summary>
    /// Контрагент, подавший заявку
    /// </summary>
    public required Counterparty Counterparty { get; set; } = null!;

    /// <summary>
    /// Идентификатор объекта недвижимости (внешний ключ)
    /// </summary>
    public Guid PropertyId { get; set; }

    /// <summary>
    /// Объект недвижимости, связанный с приложением
    /// </summary>
    public required RealEstateProperty Property { get; set; } = null!;

    /// <summary>
    /// Тип операции: покупка или продажа
    /// </summary>
    public required RequestType Type { get; set; }

    /// <summary>
    /// Денежная сумма для подачи заявки в рублях
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    public required DateTime Date { get; set; }
}
