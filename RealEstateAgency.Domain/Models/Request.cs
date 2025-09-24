using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Models;

/// <summary>
/// Application for a real estate transaction
/// It is an agreement between the counterparty and the agency.
/// </summary>
public class Request
{
    /// <summary>
    /// The unique identifier of the application
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// The counterparty who submitted the application
    /// </summary>
    public required Counterparty Counterparty { get; set; }

    /// <summary>
    /// The real estate object associated with the application
    /// </summary>
    public required RealEstateProperty Property { get; set; }

    /// <summary>
    /// Type of operation: purchase or sale
    /// </summary>
    public required RequestType Type { get; set; }

    /// <summary>
    /// The amount of money for the application in rubles
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Application submission date
    /// </summary>
    public required DateTime Date { get; set; }
}
