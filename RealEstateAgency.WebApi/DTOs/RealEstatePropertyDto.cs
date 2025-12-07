using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.WebApi.DTOs;

/// <summary>
/// DTO для отображения объекта недвижимости
/// </summary>
public class RealEstatePropertyDto
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public PropertyType Type { get; set; }

    /// <summary>
    /// Назначение недвижимости
    /// </summary>
    public PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Количество этажей в здании
    /// </summary>
    public int? TotalFloors { get; set; }

    /// <summary>
    /// Общая площадь (кв.м)
    /// </summary>
    public double TotalArea { get; set; }

    /// <summary>
    /// Количество комнат
    /// </summary>
    public int? RoomsCount { get; set; }

    /// <summary>
    /// Высота потолков (м)
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Наличие обременений
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}

/// <summary>
/// DTO для создания объекта недвижимости
/// </summary>
public class CreateRealEstatePropertyDto
{
    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public PropertyType Type { get; set; }

    /// <summary>
    /// Назначение недвижимости
    /// </summary>
    public PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Количество этажей в здании
    /// </summary>
    public int? TotalFloors { get; set; }

    /// <summary>
    /// Общая площадь (кв.м)
    /// </summary>
    public double TotalArea { get; set; }

    /// <summary>
    /// Количество комнат
    /// </summary>
    public int? RoomsCount { get; set; }

    /// <summary>
    /// Высота потолков (м)
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Наличие обременений
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}

/// <summary>
/// DTO для обновления объекта недвижимости
/// </summary>
public class UpdateRealEstatePropertyDto
{
    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public PropertyType Type { get; set; }

    /// <summary>
    /// Назначение недвижимости
    /// </summary>
    public PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Количество этажей в здании
    /// </summary>
    public int? TotalFloors { get; set; }

    /// <summary>
    /// Общая площадь (кв.м)
    /// </summary>
    public double TotalArea { get; set; }

    /// <summary>
    /// Количество комнат
    /// </summary>
    public int? RoomsCount { get; set; }

    /// <summary>
    /// Высота потолков (м)
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Наличие обременений
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}
