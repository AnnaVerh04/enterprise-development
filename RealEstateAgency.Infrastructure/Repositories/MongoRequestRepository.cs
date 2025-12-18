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
        return await context.Requests.ToListAsync();
    }

    public async Task<Request?> GetByIdAsync(Guid id)
    {
        return await context.Requests.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Request> AddAsync(Request request)
    {
        request.Id = Guid.NewGuid();

        context.Requests.Add(request);
        await context.SaveChangesAsync();
        return request;
    }

    public async Task<Request?> UpdateAsync(Guid id, Request request)
    {
        var existing = await context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        if (existing == null)
            return null;

        existing.Counterparty = request.Counterparty;
        existing.Property = request.Property;
        existing.Type = request.Type;
        existing.Amount = request.Amount;
        existing.Date = request.Date;

        await context.SaveChangesAsync();
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
}
