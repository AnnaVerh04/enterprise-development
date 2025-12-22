using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Infrastructure.Repositories;

/// <summary>
/// In-memory реализация репозитория заявок
/// </summary>
public class InMemoryRequestRepository(
    ICounterpartyRepository counterpartyRepository,
    IRealEstatePropertyRepository propertyRepository) : IRequestRepository
{
    private readonly List<Request> _requests = [];
    private bool _seeded;

    private async Task EnsureSeededAsync()
    {
        if (_seeded) return;
        _seeded = true;
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        var counterparties = (await counterpartyRepository.GetAllAsync()).ToList();
        var properties = (await propertyRepository.GetAllAsync()).ToList();

        var seedData = new[]
        {
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), CounterpartyId = counterparties[0].Id, Counterparty = counterparties[0], PropertyId = properties[0].Id, Property = properties[0], Type = RequestType.Sale, Amount = 25000000.00m, Date = new DateTime(2024, 1, 15) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), CounterpartyId = counterparties[1].Id, Counterparty = counterparties[1], PropertyId = properties[1].Id, Property = properties[1], Type = RequestType.Sale, Amount = 18000000.00m, Date = new DateTime(2024, 2, 20) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), CounterpartyId = counterparties[3].Id, Counterparty = counterparties[3], PropertyId = properties[3].Id, Property = properties[3], Type = RequestType.Sale, Amount = 42000000.00m, Date = new DateTime(2024, 3, 10) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), CounterpartyId = counterparties[6].Id, Counterparty = counterparties[6], PropertyId = properties[5].Id, Property = properties[5], Type = RequestType.Sale, Amount = 35000000.00m, Date = new DateTime(2024, 4, 5) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000005"), CounterpartyId = counterparties[8].Id, Counterparty = counterparties[8], PropertyId = properties[7].Id, Property = properties[7], Type = RequestType.Sale, Amount = 32000000.00m, Date = new DateTime(2024, 5, 12) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000006"), CounterpartyId = counterparties[10].Id, Counterparty = counterparties[10], PropertyId = properties[9].Id, Property = properties[9], Type = RequestType.Sale, Amount = 1500000.00m, Date = new DateTime(2024, 6, 8) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000007"), CounterpartyId = counterparties[11].Id, Counterparty = counterparties[11], PropertyId = properties[11].Id, Property = properties[11], Type = RequestType.Sale, Amount = 85000000.00m, Date = new DateTime(2024, 7, 25) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000008"), CounterpartyId = counterparties[2].Id, Counterparty = counterparties[2], PropertyId = properties[2].Id, Property = properties[2], Type = RequestType.Purchase, Amount = 22000000.00m, Date = new DateTime(2024, 1, 20) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-000000000009"), CounterpartyId = counterparties[4].Id, Counterparty = counterparties[4], PropertyId = properties[4].Id, Property = properties[4], Type = RequestType.Purchase, Amount = 15000000.00m, Date = new DateTime(2024, 2, 25) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-00000000000a"), CounterpartyId = counterparties[5].Id, Counterparty = counterparties[5], PropertyId = properties[6].Id, Property = properties[6], Type = RequestType.Purchase, Amount = 28000000.00m, Date = new DateTime(2024, 3, 15) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-00000000000b"), CounterpartyId = counterparties[7].Id, Counterparty = counterparties[7], PropertyId = properties[8].Id, Property = properties[8], Type = RequestType.Purchase, Amount = 25000000.00m, Date = new DateTime(2024, 4, 18) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-00000000000c"), CounterpartyId = counterparties[9].Id, Counterparty = counterparties[9], PropertyId = properties[10].Id, Property = properties[10], Type = RequestType.Purchase, Amount = 1800000.00m, Date = new DateTime(2024, 5, 22) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-00000000000d"), CounterpartyId = counterparties[2].Id, Counterparty = counterparties[2], PropertyId = properties[12].Id, Property = properties[12], Type = RequestType.Purchase, Amount = 60000000.00m, Date = new DateTime(2024, 6, 30) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-00000000000e"), CounterpartyId = counterparties[1].Id, Counterparty = counterparties[1], PropertyId = properties[0].Id, Property = properties[0], Type = RequestType.Purchase, Amount = 24000000.00m, Date = new DateTime(2024, 8, 10) },
            new Request { Id = Guid.Parse("20000000-0000-0000-0000-00000000000f"), CounterpartyId = counterparties[3].Id, Counterparty = counterparties[3], PropertyId = properties[1].Id, Property = properties[1], Type = RequestType.Sale, Amount = 19000000.00m, Date = new DateTime(2024, 9, 5) }
        };

        _requests.AddRange(seedData);
    }

    public async Task<IEnumerable<Request>> GetAllAsync()
    {
        await EnsureSeededAsync();
        return _requests;
    }

    public async Task<Request?> GetByIdAsync(Guid id)
    {
        await EnsureSeededAsync();
        return _requests.FirstOrDefault(r => r.Id == id);
    }

    public async Task<Request> AddAsync(Request request)
    {
        await EnsureSeededAsync();
        request.Id = Guid.NewGuid();
        _requests.Add(request);
        return request;
    }

    public async Task<Request?> UpdateAsync(Guid id, Request request)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) return null;

        existing.CounterpartyId = request.CounterpartyId;
        existing.Counterparty = request.Counterparty;
        existing.PropertyId = request.PropertyId;
        existing.Property = request.Property;
        existing.Type = request.Type;
        existing.Amount = request.Amount;
        existing.Date = request.Date;
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var request = await GetByIdAsync(id);
        if (request == null) return false;

        _requests.Remove(request);
        return true;
    }
}
