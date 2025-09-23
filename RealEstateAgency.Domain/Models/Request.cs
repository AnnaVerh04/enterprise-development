using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Models;

public class Request
{
    public required int Id { get; set; }
    public required Counterparty Counterparty { get; set; }
    public required RealEstateProperty Property { get; set; }
    public required RequestType Type { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
}
