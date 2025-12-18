using RealEstateAgency.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.Contracts.Dto;

/// <summary>
/// DTO для создания заявки
/// </summary>
public class CreateRequestDto
{
    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    [Required(ErrorMessage = "Идентификатор контрагента обязателен")]
    public Guid CounterpartyId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    [Required(ErrorMessage = "Идентификатор объекта недвижимости обязателен")]
    public Guid PropertyId { get; set; }

    /// <summary>
    /// Тип заявки (покупка/продажа)
    /// </summary>
    [Required(ErrorMessage = "Тип заявки обязателен")]
    public RequestType Type { get; set; }

    /// <summary>
    /// Сумма сделки
    /// </summary>
    [Required(ErrorMessage = "Сумма сделки обязательна")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Сумма сделки должна быть положительной")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    [Required(ErrorMessage = "Дата подачи заявки обязательна")]
    public DateTime Date { get; set; }
}
