using RealEstateAgency.Domain.Enums;
using RealEstateAgency.WebApi.DTOs;
using RealEstateAgency.WebApi.Repositories;

namespace RealEstateAgency.WebApi.Services;

/// <summary>
/// Интерфейс сервиса аналитики
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    public IEnumerable<string> GetSellersInPeriod(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получить топ-5 клиентов по количеству заявок (покупка и продажа отдельно)
    /// </summary>
    public Top5ClientsResultDto GetTop5ClientsByRequestCount();

    /// <summary>
    /// Получить статистику заявок по типам недвижимости
    /// </summary>
    public IEnumerable<PropertyTypeStatisticsDto> GetRequestCountByPropertyType();

    /// <summary>
    /// Получить клиентов с заявками минимальной стоимости
    /// </summary>
    public ClientWithMinAmountDto GetClientsWithMinAmount();

    /// <summary>
    /// Получить клиентов, ищущих определённый тип недвижимости
    /// </summary>
    public IEnumerable<string> GetClientsSeekingPropertyType(PropertyType propertyType);
}

/// <summary>
/// Реализация сервиса аналитики
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IRequestRepository _requestRepository;

    public AnalyticsService(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    public IEnumerable<string> GetSellersInPeriod(DateTime startDate, DateTime endDate)
    {
        return _requestRepository.GetAll()
            .Where(r => r.Type == RequestType.Sale &&
                        r.Date >= startDate &&
                        r.Date <= endDate)
            .Select(r => r.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();
    }

    /// <summary>
    /// Получить топ-5 клиентов по количеству заявок
    /// </summary>
    public Top5ClientsResultDto GetTop5ClientsByRequestCount()
    {
        var requests = _requestRepository.GetAll().ToList();

        var topPurchaseClients = requests
            .Where(r => r.Type == RequestType.Purchase)
            .GroupBy(r => r.Counterparty)
            .Select(g => new TopClientDto
            {
                FullName = g.Key.FullName,
                RequestCount = g.Count()
            })
            .OrderByDescending(x => x.RequestCount)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToList();

        var topSaleClients = requests
            .Where(r => r.Type == RequestType.Sale)
            .GroupBy(r => r.Counterparty)
            .Select(g => new TopClientDto
            {
                FullName = g.Key.FullName,
                RequestCount = g.Count()
            })
            .OrderByDescending(x => x.RequestCount)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToList();

        return new Top5ClientsResultDto
        {
            TopPurchaseClients = topPurchaseClients,
            TopSaleClients = topSaleClients
        };
    }

    /// <summary>
    /// Получить статистику заявок по типам недвижимости
    /// </summary>
    public IEnumerable<PropertyTypeStatisticsDto> GetRequestCountByPropertyType()
    {
        return _requestRepository.GetAll()
            .GroupBy(r => r.Property.Type)
            .Select(g => new PropertyTypeStatisticsDto
            {
                PropertyType = g.Key,
                RequestCount = g.Count()
            })
            .OrderBy(x => x.PropertyType)
            .ToList();
    }

    /// <summary>
    /// Получить клиентов с минимальной суммой заявки
    /// </summary>
    public ClientWithMinAmountDto GetClientsWithMinAmount()
    {
        var requests = _requestRepository.GetAll().ToList();
        var minAmount = requests.Min(r => r.Amount);

        var clients = requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        return new ClientWithMinAmountDto
        {
            FullName = string.Join(", ", clients),
            MinAmount = minAmount
        };
    }

    /// <summary>
    /// Получить клиентов, ищущих определённый тип недвижимости
    /// </summary>
    public IEnumerable<string> GetClientsSeekingPropertyType(PropertyType propertyType)
    {
        return _requestRepository.GetAll()
            .Where(r => r.Type == RequestType.Purchase &&
                        r.Property.Type == propertyType)
            .Select(r => r.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();
    }
}
