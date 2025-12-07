using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// Интерфейс репозитория контрагентов
/// </summary>
public interface ICounterpartyRepository
{
    IEnumerable<Counterparty> GetAll();
    Counterparty? GetById(int id);
    Counterparty Add(Counterparty counterparty);
    Counterparty? Update(int id, Counterparty counterparty);
    bool Delete(int id);
}

/// <summary>
/// Интерфейс репозитория объектов недвижимости
/// </summary>
public interface IRealEstatePropertyRepository
{
    IEnumerable<RealEstateProperty> GetAll();
    RealEstateProperty? GetById(int id);
    RealEstateProperty Add(RealEstateProperty property);
    RealEstateProperty? Update(int id, RealEstateProperty property);
    bool Delete(int id);
}

/// <summary>
/// Интерфейс репозитория заявок
/// </summary>
public interface IRequestRepository
{
    IEnumerable<Request> GetAll();
    Request? GetById(int id);
    Request Add(Request request);
    Request? Update(int id, Request request);
    bool Delete(int id);
}
