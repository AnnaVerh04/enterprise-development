using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория объектов недвижимости
/// </summary>
public interface IRealEstatePropertyRepository : IRepository<RealEstateProperty>;
