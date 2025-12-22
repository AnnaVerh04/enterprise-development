using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.Infrastructure.Persistence;

namespace RealEstateAgency.Infrastructure.Repositories;

/// <summary>
/// MongoDB реализация репозитория заявок с использованием EF Core
/// </summary>
public class MongoRequestRepository(RealEstateDbContext context) : IRequestRepository
{
    public async Task<IEnumerable<Request>> GetAllAsync()
    {
        var requests = await context.Requests.ToListAsync();
        await PopulateNavigationPropertiesAsync(requests);
        return requests;
    }

    public async Task<Request?> GetByIdAsync(Guid id)
    {
        var request = await context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        if (request != null)
        {
            await PopulateNavigationPropertiesAsync([request]);
        }
        return request;
    }

    public async Task<Request> AddAsync(Request request)
    {
        request.Id = Guid.NewGuid();

        if (request.CounterpartyId == Guid.Empty && request.Counterparty != null)
            request.CounterpartyId = request.Counterparty.Id;
        if (request.PropertyId == Guid.Empty && request.Property != null)
            request.PropertyId = request.Property.Id;

        context.Requests.Add(request);
        await context.SaveChangesAsync();
        return request;
    }

    public async Task<Request?> UpdateAsync(Guid id, Request request)
    {
        var existing = await context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        if (existing == null)
            return null;

        existing.CounterpartyId = request.CounterpartyId;
        existing.PropertyId = request.PropertyId;
        existing.Type = request.Type;
        existing.Amount = request.Amount;
        existing.Date = request.Date;

        await context.SaveChangesAsync();

        existing.Counterparty = request.Counterparty;
        existing.Property = request.Property;

        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var request = await context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        if (request == null)
            return false;

        context.Requests.Remove(request);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Загружает связанные Counterparty и Property для списка заявок
    /// </summary>
    private async Task PopulateNavigationPropertiesAsync(IEnumerable<Request> requests)
    {
        var requestList = requests.ToList();
        if (requestList.Count == 0) return;

        var counterpartyIds = requestList.Select(r => r.CounterpartyId).Distinct().ToList();
        var propertyIds = requestList.Select(r => r.PropertyId).Distinct().ToList();

        var counterparties = await context.Counterparties
            .Where(c => counterpartyIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id);

        var properties = await context.Properties
            .Where(p => propertyIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var request in requestList)
        {
            if (counterparties.TryGetValue(request.CounterpartyId, out var counterparty))
                request.Counterparty = counterparty;

            if (properties.TryGetValue(request.PropertyId, out var property))
                request.Property = property;
        }
    }
}
