namespace RealEstateAgency.Domain.Models;

/// <summary>
/// The counterparty of the real estate agency
/// An individual involved in real estate transactions
/// </summary>
public class Counterparty
{
    /// <summary>
    /// The unique identifier of the counterparty
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The counterparty's full name in the "Last Name, First Name, Patronymic" format
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Passport number for identification
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Contact phone number for communication
    /// </summary>
    public required string PhoneNumber { get; set; }
}
