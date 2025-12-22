using System.ComponentModel.DataAnnotations;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Contracts.Dto;

/// <summary>
/// DTO для создания объекта недвижимости
/// </summary>
public class CreateRealEstatePropertyDto
{
    /// <summary>
    /// Тип недвижимости
    /// </summary>
    [Required(ErrorMessage = "Тип недвижимости обязателен")]
    public PropertyType Type { get; set; }

    /// <summary>
    /// Назначение недвижимости
    /// </summary>
    [Required(ErrorMessage = "Назначение недвижимости обязательно")]
    public PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер
    /// </summary>
    [Required(ErrorMessage = "Кадастровый номер обязателен")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Кадастровый номер должен содержать от 5 до 50 символов")]
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "Адрес должен содержать от 5 до 500 символов")]
    public required string Address { get; set; }

    /// <summary>
    /// Количество этажей в здании
    /// </summary>
    [Range(1, 200, ErrorMessage = "Количество этажей должно быть от 1 до 200")]
    public int? TotalFloors { get; set; }

    /// <summary>
    /// Общая площадь (кв.м)
    /// </summary>
    [Required(ErrorMessage = "Общая площадь обязательна")]
    [Range(0.1, 1000000, ErrorMessage = "Площадь должна быть от 0.1 до 1000000 кв.м")]
    public double TotalArea { get; set; }

    /// <summary>
    /// Количество комнат
    /// </summary>
    [Range(1, 100, ErrorMessage = "Количество комнат должно быть от 1 до 100")]
    public int? RoomsCount { get; set; }

    /// <summary>
    /// Высота потолков (м)
    /// </summary>
    [Range(1.5, 20, ErrorMessage = "Высота потолков должна быть от 1.5 до 20 м")]
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения
    /// </summary>
    [Range(-5, 200, ErrorMessage = "Этаж должен быть от -5 до 200")]
    public int? Floor { get; set; }

    /// <summary>
    /// Наличие обременений
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}
