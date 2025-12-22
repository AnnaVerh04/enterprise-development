namespace RealEstateAgency.Domain.Enums;

/// <summary>
/// Тип недвижимости
/// Классифицирует имущество по физическим характеристикам
/// </summary>
public enum PropertyType
{
    /// <summary>
    /// Квартира в многоквартирном доме
    /// </summary>
    Apartment,

    /// <summary>
    /// Отдельно стоящее многоквартирное здание
    /// </summary>
    House,

    /// <summary>
    /// Блокированный многоквартирный дом с отдельными входами
    /// </summary>
    Townhouse,

    /// <summary>
    /// Коммерческие помещения для ведения бизнеса
    /// </summary>
    Commercial,

    /// <summary>
    /// Складские или производственные помещения
    /// </summary>
    Warehouse,

    /// <summary>
    /// Место для парковки транспортных средств
    /// </summary>
    ParkingSpace
}
