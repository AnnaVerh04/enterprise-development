using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Interfaces;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Реализация сервиса аналитики
/// </summary>
public class AnalyticsService(
    IRequestRepository requestRepository,
    ICounterpartyRepository counterpartyRepository,
    IRealEstatePropertyRepository propertyRepository) : IAnalyticsService
{

    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    public async Task<IEnumerable<string>> GetSellersInPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var requests = (await requestRepository.GetAllAsync()).ToList();
        var counterparties = (await counterpartyRepository.GetAllAsync()).ToDictionary(c => c.Id);

        return
        [
            .. requests
                .Where(r => r.Type == RequestType.Sale &&
                            r.Date >= startDate &&
                            r.Date <= endDate)
                .Select(r => counterparties.TryGetValue(r.CounterpartyId, out var c) ? c.FullName : r.Counterparty?.FullName)
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .Order()
        ];
    }

    /// <summary>
    /// Получить топ-5 клиентов по количеству заявок
    /// </summary>
    public async Task<Top5ClientsResultDto> GetTop5ClientsByRequestCountAsync()
    {
        var requests = (await requestRepository.GetAllAsync()).ToList();
        var counterparties = (await counterpartyRepository.GetAllAsync()).ToDictionary(c => c.Id);

        var topPurchaseClients = requests
            .Where(r => r.Type == RequestType.Purchase)
            .GroupBy(r => r.CounterpartyId)
            .Select(g => new TopClientDto
            {
                FullName = counterparties.TryGetValue(g.Key, out var c) ? c.FullName : g.First().Counterparty?.FullName ?? "",
                RequestCount = g.Count()
            })
            .Where(x => !string.IsNullOrEmpty(x.FullName))
            .OrderByDescending(x => x.RequestCount)
            .ThenBy(x => x.FullName)
            .Take(5);

        var topSaleClients = requests
            .Where(r => r.Type == RequestType.Sale)
            .GroupBy(r => r.CounterpartyId)
            .Select(g => new TopClientDto
            {
                FullName = counterparties.TryGetValue(g.Key, out var c) ? c.FullName : g.First().Counterparty?.FullName ?? "",
                RequestCount = g.Count()
            })
            .Where(x => !string.IsNullOrEmpty(x.FullName))
            .OrderByDescending(x => x.RequestCount)
            .ThenBy(x => x.FullName)
            .Take(5);

        return new Top5ClientsResultDto
        {
            TopPurchaseClients = [.. topPurchaseClients],
            TopSaleClients = [.. topSaleClients]
        };
    }

    /// <summary>
    /// Получить статистику заявок по типам недвижимости
    /// </summary>
    public async Task<IEnumerable<PropertyTypeStatisticsDto>> GetRequestCountByPropertyTypeAsync()
    {
        var requests = (await requestRepository.GetAllAsync()).ToList();
        var properties = (await propertyRepository.GetAllAsync()).ToDictionary(p => p.Id);

        return
        [
            .. requests
                .Select(r => new
                {
                    PropertyType = properties.TryGetValue(r.PropertyId, out var p) ? p.Type : r.Property?.Type ?? default
                })
                .GroupBy(x => x.PropertyType)
                .Select(g => new PropertyTypeStatisticsDto
                {
                    PropertyType = g.Key,
                    RequestCount = g.Count()
                })
                .OrderBy(x => x.PropertyType)
        ];
    }

    /// <summary>
    /// Получить клиентов с минимальной суммой заявки
    /// </summary>
    public async Task<ClientWithMinAmountDto> GetClientsWithMinAmountAsync()
    {
        var requests = (await requestRepository.GetAllAsync()).ToList();
        var counterparties = (await counterpartyRepository.GetAllAsync()).ToDictionary(c => c.Id);

        var minAmount = requests.Min(r => r.Amount);

        var clients = requests
            .Where(r => r.Amount == minAmount)
            .Select(r => counterparties.TryGetValue(r.CounterpartyId, out var c) ? c.FullName : r.Counterparty?.FullName)
            .Where(name => !string.IsNullOrEmpty(name))
            .Distinct()
            .Order();

        return new ClientWithMinAmountDto
        {
            FullName = string.Join(", ", clients),
            MinAmount = minAmount
        };
    }

    /// <summary>
    /// Получить клиентов, ищущих определённый тип недвижимости
    /// </summary>
    public async Task<IEnumerable<string>> GetClientsSeekingPropertyTypeAsync(PropertyType propertyType)
    {
        var requests = (await requestRepository.GetAllAsync()).ToList();
        var counterparties = (await counterpartyRepository.GetAllAsync()).ToDictionary(c => c.Id);
        var properties = (await propertyRepository.GetAllAsync()).ToDictionary(p => p.Id);

        return
        [
            .. requests
                .Where(r => r.Type == RequestType.Purchase &&
                            (properties.TryGetValue(r.PropertyId, out var p) ? p.Type : r.Property?.Type ?? default) == propertyType)
                .Select(r => counterparties.TryGetValue(r.CounterpartyId, out var c) ? c.FullName : r.Counterparty?.FullName)
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .Order()
        ];
    }
}
