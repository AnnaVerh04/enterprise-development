using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Models;

/// <summary>
/// The real estate object
/// Describes the physical characteristics of the property
/// </summary>
public class RealEstateProperty
{
    /// <summary>
    /// The unique identifier of the object
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Property type
    /// </summary>
    public required PropertyType Type { get; set; }

    /// <summary>
    /// Purpose of the property
    /// </summary>
    public required PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// A unique identifier in the state registry
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// The physical address of the object location
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Total number of floors of the building
    /// </summary>
    public int? TotalFloors { get; set; }

    /// <summary>
    /// The total area of the facility in square meters
    /// </summary>
    public required double TotalArea { get; set; }

    /// <summary>
    /// Number of rooms in the facility
    /// </summary>
    public int? RoomsCount { get; set; }

    /// <summary>
    /// Ceiling height in meters
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// The floor of the object location
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// The presence of legal encumbrances (collateral, arrest, mortgage)
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}
