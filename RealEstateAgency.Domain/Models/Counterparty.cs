namespace RealEstateAgency.Domain.Models;

public class Counterparty
{
    public required int Id { get; set; }
    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string PhoneNumber { get; set; }
}
