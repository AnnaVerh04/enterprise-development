namespace RealEstateAgency.Domain.Enums;

/// <summary>
/// Тип заявления в агентство недвижимости
/// Определяет направление сделки с недвижимостью
/// </summary>
public enum RequestType
{
    /// <summary>
    /// Заявка на покупку недвижимости
    /// </summary>
    Purchase,

    /// <summary>
    /// Заявка на продажу недвижимости
    /// </summary>
    Sale
}
