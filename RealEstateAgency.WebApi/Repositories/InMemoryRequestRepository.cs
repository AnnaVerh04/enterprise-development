using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// In-memory реализация репозитория заявок
/// </summary>
public class InMemoryRequestRepository : IRequestRepository
{
    private readonly List<Request> _requests = [];
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly IRealEstatePropertyRepository _propertyRepository;
    private int _nextId = 1;

    public InMemoryRequestRepository(
        ICounterpartyRepository counterpartyRepository,
        IRealEstatePropertyRepository propertyRepository)
    {
        _counterpartyRepository = counterpartyRepository;
        _propertyRepository = propertyRepository;
        SeedData();
    }

    private void SeedData()
    {
        var counterparties = _counterpartyRepository.GetAll().ToList();
        var properties = _propertyRepository.GetAll().ToList();

        var seedData = new[]
        {
            new Request { Id = 1, Counterparty = counterparties[0], Property = properties[0], Type = RequestType.Sale, Amount = 25000000.00m, Date = new DateTime(2024, 1, 15) },
            new Request { Id = 2, Counterparty = counterparties[1], Property = properties[1], Type = RequestType.Sale, Amount = 18000000.00m, Date = new DateTime(2024, 2, 20) },
            new Request { Id = 3, Counterparty = counterparties[3], Property = properties[3], Type = RequestType.Sale, Amount = 42000000.00m, Date = new DateTime(2024, 3, 10) },
            new Request { Id = 4, Counterparty = counterparties[6], Property = properties[5], Type = RequestType.Sale, Amount = 35000000.00m, Date = new DateTime(2024, 4, 5) },
            new Request { Id = 5, Counterparty = counterparties[8], Property = properties[7], Type = RequestType.Sale, Amount = 32000000.00m, Date = new DateTime(2024, 5, 12) },
            new Request { Id = 6, Counterparty = counterparties[10], Property = properties[9], Type = RequestType.Sale, Amount = 1500000.00m, Date = new DateTime(2024, 6, 8) },
            new Request { Id = 7, Counterparty = counterparties[11], Property = properties[11], Type = RequestType.Sale, Amount = 85000000.00m, Date = new DateTime(2024, 7, 25) },
            new Request { Id = 8, Counterparty = counterparties[2], Property = properties[2], Type = RequestType.Purchase, Amount = 22000000.00m, Date = new DateTime(2024, 1, 20) },
            new Request { Id = 9, Counterparty = counterparties[4], Property = properties[4], Type = RequestType.Purchase, Amount = 15000000.00m, Date = new DateTime(2024, 2, 25) },
            new Request { Id = 10, Counterparty = counterparties[5], Property = properties[6], Type = RequestType.Purchase, Amount = 28000000.00m, Date = new DateTime(2024, 3, 15) },
            new Request { Id = 11, Counterparty = counterparties[7], Property = properties[8], Type = RequestType.Purchase, Amount = 25000000.00m, Date = new DateTime(2024, 4, 18) },
            new Request { Id = 12, Counterparty = counterparties[9], Property = properties[10], Type = RequestType.Purchase, Amount = 1800000.00m, Date = new DateTime(2024, 5, 22) },
            new Request { Id = 13, Counterparty = counterparties[2], Property = properties[12], Type = RequestType.Purchase, Amount = 60000000.00m, Date = new DateTime(2024, 6, 30) },
            new Request { Id = 14, Counterparty = counterparties[1], Property = properties[0], Type = RequestType.Purchase, Amount = 24000000.00m, Date = new DateTime(2024, 8, 10) },
            new Request { Id = 15, Counterparty = counterparties[3], Property = properties[1], Type = RequestType.Sale, Amount = 19000000.00m, Date = new DateTime(2024, 9, 5) }
        };

        _requests.AddRange(seedData);
        _nextId = seedData.Length + 1;
    }

    public IEnumerable<Request> GetAll() => _requests;

    public Request? GetById(int id) => _requests.FirstOrDefault(r => r.Id == id);

    public Request Add(Request request)
    {
        request.Id = _nextId++;
        _requests.Add(request);
        return request;
    }

    public Request? Update(int id, Request request)
    {
        var existing = GetById(id);
        if (existing == null) return null;

        existing.Counterparty = request.Counterparty;
        existing.Property = request.Property;
        existing.Type = request.Type;
        existing.Amount = request.Amount;
        existing.Date = request.Date;
        return existing;
    }

    public bool Delete(int id)
    {
        var request = GetById(id);
        if (request == null) return false;

        _requests.Remove(request);
        return true;
    }
}
