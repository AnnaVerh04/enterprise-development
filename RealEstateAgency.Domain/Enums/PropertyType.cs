namespace RealEstateAgency.Domain.Enums;

/// <summary>
/// Property type
/// Classifies property by physical characteristics
/// </summary>
public enum PropertyType
{
    /// <summary>
    /// Apartment in an apartment building
    /// </summary>
    Apartment,

    /// <summary>
    /// Detached apartment building
    /// </summary>
    House,

    /// <summary>
    /// A blockaded apartment building with separate entrances
    /// </summary>
    Townhouse,

    /// <summary>
    /// Commercial premises for business
    /// </summary>
    Commercial,

    /// <summary>
    /// Warehouse or production premises
    /// </summary>
    Warehouse,

    /// <summary>
    /// A place for parking vehicles
    /// </summary>
    ParkingSpace
}
