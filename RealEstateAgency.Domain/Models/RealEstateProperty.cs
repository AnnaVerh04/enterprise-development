using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Models;

/// <summary>
/// Объект недвижимости
/// Описывает физические характеристики объекта недвижимости
/// </summary>
public class RealEstateProperty
{
    /// <summary>
    /// Уникальный идентификатор объекта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public required PropertyType Type { get; set; }

    /// <summary>
    /// Назначение объекта недвижимости
    /// </summary>
    public required PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Уникальный идентификатор в государственном реестре
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Физический адрес местоположения объекта
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Общее количество этажей в здании
    /// </summary>
    public int? TotalFloors { get; set; }

    /// <summary>
    /// Общая площадь объекта в квадратных метрах
    /// </summary>
    public required double TotalArea { get; set; }

    /// <summary>
    /// Количество комнат в комплексе
    /// </summary>
    public int? RoomsCount { get; set; }

    /// <summary>
    /// Высота потолков в метрах
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения объекта
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Наличие юридических обременений (залог, арест, ипотека)
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}
