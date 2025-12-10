using MongoDB.Driver;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// MongoDB реализация репозитория заявок
/// </summary>
public class MongoRequestRepository : IRequestRepository
{
    private readonly IMongoCollection<Request> _collection;
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly IRealEstatePropertyRepository _propertyRepository;

    public MongoRequestRepository(
        IMongoDatabase database,
        ICounterpartyRepository counterpartyRepository,
        IRealEstatePropertyRepository propertyRepository)
    {
        _collection = database.GetCollection<Request>("requests");
        _counterpartyRepository = counterpartyRepository;
        _propertyRepository = propertyRepository;
    }

    public IEnumerable<Request> GetAll()
    {
        return _collection.Find(_ => true).ToList();
    }

    public Request? GetById(int id)
    {
        return _collection.Find(r => r.Id == id).FirstOrDefault();
    }

    public Request Add(Request request)
    {
        var maxId = _collection.Find(_ => true)
            .SortByDescending(r => r.Id)
            .FirstOrDefault()?.Id ?? 0;
        request.Id = maxId + 1;

        _collection.InsertOne(request);
        return request;
    }

    public Request? Update(int id, Request request)
    {
        var filter = Builders<Request>.Filter.Eq(r => r.Id, id);
        var update = Builders<Request>.Update
            .Set(r => r.Counterparty, request.Counterparty)
            .Set(r => r.Property, request.Property)
            .Set(r => r.Type, request.Type)
            .Set(r => r.Amount, request.Amount)
            .Set(r => r.Date, request.Date);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount == 0)
            return null;

        return GetById(id);
    }

    public bool Delete(int id)
    {
        var result = _collection.DeleteOne(r => r.Id == id);
        return result.DeletedCount > 0;
    }
}
