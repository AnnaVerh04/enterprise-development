using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.Infrastructure.Persistence;

namespace RealEstateAgency.Infrastructure.Repositories;

/// <summary>
/// MongoDB реализация репозитория контрагентов с использованием EF Core
/// </summary>
public class MongoCounterpartyRepository(RealEstateDbContext context) : ICounterpartyRepository
{
    public async Task<IEnumerable<Counterparty>> GetAllAsync()
    {
        return await context.Counterparties.ToListAsync();
    }

    public async Task<Counterparty?> GetByIdAsync(Guid id)
    {
        return await context.Counterparties.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Counterparty> AddAsync(Counterparty counterparty)
    {
        counterparty.Id = Guid.NewGuid();

        context.Counterparties.Add(counterparty);
        await context.SaveChangesAsync();
        return counterparty;
    }

    public async Task<Counterparty?> UpdateAsync(Guid id, Counterparty counterparty)
    {
        var existing = await context.Counterparties.FirstOrDefaultAsync(c => c.Id == id);
        if (existing == null)
            return null;

        existing.FullName = counterparty.FullName;
        existing.PassportNumber = counterparty.PassportNumber;
        existing.PhoneNumber = counterparty.PhoneNumber;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var counterparty = await context.Counterparties.FirstOrDefaultAsync(c => c.Id == id);
        if (counterparty == null)
            return false;

        context.Counterparties.Remove(counterparty);
        await context.SaveChangesAsync();
        return true;
    }
}
