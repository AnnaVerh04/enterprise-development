using MongoDB.Driver;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// MongoDB реализация репозитория объектов недвижимости
/// </summary>
public class MongoRealEstatePropertyRepository : IRealEstatePropertyRepository
{
    private readonly IMongoCollection<RealEstateProperty> _collection;

    public MongoRealEstatePropertyRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<RealEstateProperty>("properties");
    }

    public IEnumerable<RealEstateProperty> GetAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    public RealEstateProperty? GetById(int id)
    {
        return _collection.Find(p => p.Id == id).FirstOrDefault();
    }

    public RealEstateProperty Add(RealEstateProperty property)
    {
        var maxId = _collection.Find(_ => true)
            .SortByDescending(p => p.Id)
            .FirstOrDefault()?.Id ?? 0;
        property.Id = maxId + 1;

        _collection.InsertOne(property);
        return property;
    }

    public RealEstateProperty? Update(int id, RealEstateProperty property)
    {
        var filter = Builders<RealEstateProperty>.Filter.Eq(p => p.Id, id);
        var update = Builders<RealEstateProperty>.Update
            .Set(p => p.Type, property.Type)
            .Set(p => p.Purpose, property.Purpose)
            .Set(p => p.CadastralNumber, property.CadastralNumber)
            .Set(p => p.Address, property.Address)
            .Set(p => p.TotalFloors, property.TotalFloors)
            .Set(p => p.TotalArea, property.TotalArea)
            .Set(p => p.RoomsCount, property.RoomsCount)
            .Set(p => p.CeilingHeight, property.CeilingHeight)
            .Set(p => p.Floor, property.Floor)
            .Set(p => p.HasEncumbrances, property.HasEncumbrances);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount == 0)
            return null;

        return GetById(id);
    }

    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}
