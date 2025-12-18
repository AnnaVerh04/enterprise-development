using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Infrastructure.Repositories;

/// <summary>
/// In-memory реализация репозитория объектов недвижимости
/// </summary>
public class InMemoryRealEstatePropertyRepository : IRealEstatePropertyRepository
{
    private readonly List<RealEstateProperty> _properties = [];

    public InMemoryRealEstatePropertyRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        var seedData = new[]
        {
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Type = PropertyType.Apartment, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:01:0001001:101", Address = "ул. Тверская, 15, кв. 34", TotalFloors = 9, TotalArea = 75.5, RoomsCount = 3, CeilingHeight = 2.7, Floor = 5, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Type = PropertyType.Apartment, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:01:0001002:102", Address = "ул. Арбат, 25, кв. 12", TotalFloors = 5, TotalArea = 45.0, RoomsCount = 2, CeilingHeight = 2.5, Floor = 3, HasEncumbrances = true },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Type = PropertyType.Apartment, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:01:0001003:103", Address = "пр-т Мира, 10, кв. 78", TotalFloors = 12, TotalArea = 90.0, RoomsCount = 4, CeilingHeight = 2.8, Floor = 8, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Type = PropertyType.House, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:02:0002001:201", Address = "Московская обл., коттеджный поселок 'Лесной', д. 12", TotalFloors = 2, TotalArea = 150.0, RoomsCount = 6, CeilingHeight = 3.0, Floor = null, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Type = PropertyType.House, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:02:0002002:202", Address = "Московская обл., д. Пушкино, ул. Садовая, 5", TotalFloors = 1, TotalArea = 80.0, RoomsCount = 4, CeilingHeight = 2.6, Floor = null, HasEncumbrances = true },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Type = PropertyType.Townhouse, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:03:0003001:301", Address = "пос. Рублево, таунхаусный комплекс 'Резиденция', к. 7", TotalFloors = 3, TotalArea = 120.0, RoomsCount = 5, CeilingHeight = 2.7, Floor = null, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Type = PropertyType.Townhouse, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:03:0003002:302", Address = "пос. Барвиха, таунхаусный комплекс 'Престиж', к. 3", TotalFloors = 2, TotalArea = 95.0, RoomsCount = 4, CeilingHeight = 2.8, Floor = null, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Type = PropertyType.Commercial, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:05:0005001:501", Address = "ул. Новый Арбат, 15, офис 300", TotalFloors = 10, TotalArea = 60.0, RoomsCount = 2, CeilingHeight = 2.8, Floor = 3, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Type = PropertyType.Commercial, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:05:0005002:502", Address = "ул. Тверская-Ямская, 8, магазин", TotalFloors = 3, TotalArea = 85.0, RoomsCount = 1, CeilingHeight = 3.2, Floor = 1, HasEncumbrances = true },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-00000000000a"), Type = PropertyType.ParkingSpace, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:06:0006001:601", Address = "ул. Садовая-Кудринская, 1, подземный паркинг, место А-15", TotalFloors = null, TotalArea = 12.5, RoomsCount = null, CeilingHeight = 2.2, Floor = -1, HasEncumbrances = true },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-00000000000b"), Type = PropertyType.ParkingSpace, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:06:0006002:602", Address = "ул. Мясницкая, 20, паркинг, место Б-07", TotalFloors = null, TotalArea = 13.0, RoomsCount = null, CeilingHeight = 2.3, Floor = -2, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-00000000000c"), Type = PropertyType.Warehouse, Purpose = PropertyPurpose.Industrial, CadastralNumber = "77:07:0007001:701", Address = "промзона 'Южные Ворота', складской комплекс №3", TotalFloors = 1, TotalArea = 500.0, RoomsCount = null, CeilingHeight = 6.0, Floor = null, HasEncumbrances = false },
            new RealEstateProperty { Id = Guid.Parse("10000000-0000-0000-0000-00000000000d"), Type = PropertyType.Warehouse, Purpose = PropertyPurpose.Industrial, CadastralNumber = "77:07:0007002:702", Address = "промзона 'Северная', склад №5", TotalFloors = 2, TotalArea = 350.0, RoomsCount = null, CeilingHeight = 5.5, Floor = null, HasEncumbrances = true }
        };

        _properties.AddRange(seedData);
    }

    public Task<IEnumerable<RealEstateProperty>> GetAllAsync() => Task.FromResult<IEnumerable<RealEstateProperty>>(_properties);

    public Task<RealEstateProperty?> GetByIdAsync(Guid id) => Task.FromResult(_properties.FirstOrDefault(p => p.Id == id));

    public Task<RealEstateProperty> AddAsync(RealEstateProperty property)
    {
        property.Id = Guid.NewGuid();
        _properties.Add(property);
        return Task.FromResult(property);
    }

    public Task<RealEstateProperty?> UpdateAsync(Guid id, RealEstateProperty property)
    {
        var existing = _properties.FirstOrDefault(p => p.Id == id);
        if (existing == null) return Task.FromResult<RealEstateProperty?>(null);

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
        return Task.FromResult<RealEstateProperty?>(existing);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var property = _properties.FirstOrDefault(p => p.Id == id);
        if (property == null) return Task.FromResult(false);

        _properties.Remove(property);
        return Task.FromResult(true);
    }
}
