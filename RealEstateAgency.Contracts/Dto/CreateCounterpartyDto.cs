using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.Contracts.Dto;

/// <summary>
/// DTO для создания контрагента
/// </summary>
public class CreateCounterpartyDto
{
    /// <summary>
    /// ФИО контрагента
    /// </summary>
    [Required(ErrorMessage = "ФИО обязательно")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "ФИО должно содержать от 2 до 200 символов")]
    public required string FullName { get; set; }

    /// <summary>
    /// Номер паспорта
    /// </summary>
    [Required(ErrorMessage = "Номер паспорта обязателен")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Номер паспорта должен содержать от 6 до 20 символов")]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Контактный телефон
    /// </summary>
    [Required(ErrorMessage = "Номер телефона обязателен")]
    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    public required string PhoneNumber { get; set; }
}
