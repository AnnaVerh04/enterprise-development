using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Models;

public class RealEstateProperty
{
    public required int Id { get; set; }
    public required PropertyType Type { get; set; }
    public required PropertyPurpose Purpose { get; set; }
    public required string CadastralNumber { get; set; }
    public required string Address { get; set; }
    public int? TotalFloors { get; set; }
    public required double TotalArea { get; set; }
    public int? RoomsCount { get; set; }
    public double? CeilingHeight { get; set; }
    public int? Floor { get; set; }
    public bool? HasEncumbrances { get; set; }
}
