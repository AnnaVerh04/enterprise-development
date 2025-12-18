using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.Infrastructure.Persistence;

namespace RealEstateAgency.Infrastructure.Repositories;

/// <summary>
/// MongoDB реализация репозитория объектов недвижимости с использованием EF Core
/// </summary>
public class MongoRealEstatePropertyRepository(RealEstateDbContext context) : IRealEstatePropertyRepository
{
    public async Task<IEnumerable<RealEstateProperty>> GetAllAsync()
    {
        return await context.Properties.ToListAsync();
    }

    public async Task<RealEstateProperty?> GetByIdAsync(Guid id)
    {
        return await context.Properties.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<RealEstateProperty> AddAsync(RealEstateProperty property)
    {
        property.Id = Guid.NewGuid();

        context.Properties.Add(property);
        await context.SaveChangesAsync();
        return property;
    }

    public async Task<RealEstateProperty?> UpdateAsync(Guid id, RealEstateProperty property)
    {
        var existing = await context.Properties.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null)
            return null;

        existing.Type = property.Type;
        existing.Purpose = property.Purpose;
        existing.CadastralNumber = property.CadastralNumber;
        existing.Address = property.Address;
        existing.TotalFloors = property.TotalFloors;
        existing.TotalArea = property.TotalArea;
        existing.RoomsCount = property.RoomsCount;
        existing.CeilingHeight = property.CeilingHeight;
        existing.Floor = property.Floor;
        existing.HasEncumbrances = property.HasEncumbrances;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var property = await context.Properties.FirstOrDefaultAsync(p => p.Id == id);
        if (property == null)
            return false;

        context.Properties.Remove(property);
        await context.SaveChangesAsync();
        return true;
    }
}
