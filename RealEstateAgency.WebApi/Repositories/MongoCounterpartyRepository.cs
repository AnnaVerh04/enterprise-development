using MongoDB.Driver;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// MongoDB реализация репозитория контрагентов
/// </summary>
public class MongoCounterpartyRepository : ICounterpartyRepository
{
    private readonly IMongoCollection<Counterparty> _collection;

    public MongoCounterpartyRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Counterparty>("counterparties");
    }

    public IEnumerable<Counterparty> GetAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    public Counterparty? GetById(int id)
    {
        return _collection.Find(c => c.Id == id).FirstOrDefault();
    }

    public Counterparty Add(Counterparty counterparty)
    {
        // Генерация нового ID
        var maxId = _collection.Find(_ => true)
            .SortByDescending(c => c.Id)
            .FirstOrDefault()?.Id ?? 0;
        counterparty.Id = maxId + 1;

        _collection.InsertOne(counterparty);
        return counterparty;
    }

    public Counterparty? Update(int id, Counterparty counterparty)
    {
        var filter = Builders<Counterparty>.Filter.Eq(c => c.Id, id);
        var update = Builders<Counterparty>.Update
            .Set(c => c.FullName, counterparty.FullName)
            .Set(c => c.PassportNumber, counterparty.PassportNumber)
            .Set(c => c.PhoneNumber, counterparty.PhoneNumber);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount == 0)
            return null;

        return GetById(id);
    }

    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}
