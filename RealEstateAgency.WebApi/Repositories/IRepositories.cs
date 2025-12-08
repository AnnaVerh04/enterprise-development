using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// Интерфейс репозитория контрагентов
/// </summary>
public interface ICounterpartyRepository
{
    public IEnumerable<Counterparty> GetAll();
    public Counterparty? GetById(int id);
    public Counterparty Add(Counterparty counterparty);
    public Counterparty? Update(int id, Counterparty counterparty);
    public bool Delete(int id);
}

/// <summary>
/// Интерфейс репозитория объектов недвижимости
/// </summary>
public interface IRealEstatePropertyRepository
{
    public IEnumerable<RealEstateProperty> GetAll();
    public RealEstateProperty? GetById(int id);
    public RealEstateProperty Add(RealEstateProperty property);
    public RealEstateProperty? Update(int id, RealEstateProperty property);
    public bool Delete(int id);
}

/// <summary>
/// Интерфейс репозитория заявок
/// </summary>
public interface IRequestRepository
{
    public IEnumerable<Request> GetAll();
    public Request? GetById(int id);
    public Request Add(Request request);
    public Request? Update(int id, Request request);
    public bool Delete(int id);
}
