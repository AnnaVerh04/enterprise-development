using System.Net.Http.Json;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.WebApi.DTOs;
using Xunit;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Интеграционные тесты аналитических запросов с реальной MongoDB
/// </summary>
[Collection("MongoDB")]
public class MongoAnalyticsTests : IClassFixture<MongoDbWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MongoAnalyticsTests(MongoDbWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Тест аналитики продавцов за период с MongoDB
    /// </summary>
    [Fact]
    public async Task GetSellersInPeriod_WorksWithMongoDB()
    {
        var seller = new CreateCounterpartyDto
        {
            FullName = "Продавец для MongoDB теста",
            PassportNumber = "5555 666666",
            PhoneNumber = "+7-555-666-77-88"
        };
        var sellerResponse = await _client.PostAsJsonAsync("/api/counterparties", seller);
        var createdSeller = await sellerResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var property = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:55:5555555:555",
            Address = "ул. Аналитическая, д. 1",
            TotalArea = 60.0
        };
        var propertyResponse = await _client.PostAsJsonAsync("/api/properties", property);
        var createdProperty = await propertyResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var saleRequest = new CreateRequestDto
        {
            CounterpartyId = createdSeller!.Id,
            PropertyId = createdProperty!.Id,
            Type = RequestType.Sale,
            Amount = 8000000.00m,
            Date = new DateTime(2024, 5, 15) 
        };
        await _client.PostAsJsonAsync("/api/requests", saleRequest);

        var response = await _client.GetAsync(
            "/api/analytics/sellers?startDate=2024-01-01&endDate=2024-12-31");

        response.EnsureSuccessStatusCode();
        var sellers = await response.Content.ReadFromJsonAsync<List<string>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(sellers);
        Assert.Contains("Продавец для MongoDB теста", sellers);
    }

    /// <summary>
    /// Тест статистики по типам недвижимости с MongoDB
    /// </summary>
    [Fact]
    public async Task GetPropertyTypeStatistics_WorksWithMongoDB()
    {
        var client = new CreateCounterpartyDto
        {
            FullName = "Клиент для статистики MongoDB",
            PassportNumber = "6666 777777",
            PhoneNumber = "+7-666-777-88-99"
        };
        var clientResponse = await _client.PostAsJsonAsync("/api/counterparties", client);
        var createdClient = await clientResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var propertyTypes = new[] { PropertyType.Apartment, PropertyType.House, PropertyType.Commercial };

        foreach (var propertyType in propertyTypes)
        {
            var property = new CreateRealEstatePropertyDto
            {
                Type = propertyType,
                Purpose = propertyType == PropertyType.Commercial ? PropertyPurpose.Commercial : PropertyPurpose.Residential,
                CadastralNumber = $"77:66:666666{(int)propertyType}:666",
                Address = $"Адрес для статистики {propertyType}",
                TotalArea = 100.0
            };
            var propResponse = await _client.PostAsJsonAsync("/api/properties", property);
            var createdProp = await propResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
                RealEstateWebApplicationFactory.JsonOptions);

            var request = new CreateRequestDto
            {
                CounterpartyId = createdClient!.Id,
                PropertyId = createdProp!.Id,
                Type = RequestType.Purchase,
                Amount = 5000000.00m,
                Date = DateTime.Now
            };
            await _client.PostAsJsonAsync("/api/requests", request);
        }

        var response = await _client.GetAsync("/api/analytics/property-type-statistics");

        response.EnsureSuccessStatusCode();
        var stats = await response.Content.ReadFromJsonAsync<List<PropertyTypeStatisticsDto>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(stats);
        Assert.True(stats.Count > 0);

        Assert.Contains(stats, s => s.PropertyType == PropertyType.Apartment);
        Assert.Contains(stats, s => s.PropertyType == PropertyType.House);
        Assert.Contains(stats, s => s.PropertyType == PropertyType.Commercial);
    }

    /// <summary>
    /// Тест поиска клиентов по типу недвижимости с MongoDB
    /// </summary>
    [Fact]
    public async Task GetClientsByPropertyType_WorksWithMongoDB()
    {
        var buyer = new CreateCounterpartyDto
        {
            FullName = "Покупатель квартиры MongoDB",
            PassportNumber = "7777 888888",
            PhoneNumber = "+7-777-888-99-00"
        };
        var buyerResponse = await _client.PostAsJsonAsync("/api/counterparties", buyer);
        var createdBuyer = await buyerResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var apartment = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:77:7777777:777",
            Address = "ул. Квартирная MongoDB, д. 1",
            TotalArea = 55.0
        };
        var apartmentResponse = await _client.PostAsJsonAsync("/api/properties", apartment);
        var createdApartment = await apartmentResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var purchaseRequest = new CreateRequestDto
        {
            CounterpartyId = createdBuyer!.Id,
            PropertyId = createdApartment!.Id,
            Type = RequestType.Purchase,
            Amount = 6000000.00m,
            Date = DateTime.Now
        };
        await _client.PostAsJsonAsync("/api/requests", purchaseRequest);

        var response = await _client.GetAsync(
            "/api/analytics/clients-by-property-type?propertyType=Apartment");

        response.EnsureSuccessStatusCode();
        var clients = await response.Content.ReadFromJsonAsync<List<string>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(clients);
        Assert.Contains("Покупатель квартиры MongoDB", clients);
    }

    /// <summary>
    /// Тест топ-5 клиентов с MongoDB
    /// </summary>
    [Fact]
    public async Task GetTop5Clients_WorksWithMongoDB()
    {
        var activeBuyer = new CreateCounterpartyDto
        {
            FullName = "Активный покупатель MongoDB",
            PassportNumber = "8888 999999",
            PhoneNumber = "+7-888-999-00-11"
        };
        var buyerResponse = await _client.PostAsJsonAsync("/api/counterparties", activeBuyer);
        var createdBuyer = await buyerResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        for (var i = 1; i <= 3; i++)
        {
            var property = new CreateRealEstatePropertyDto
            {
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = $"77:88:888888{i}:888",
                Address = $"ул. Топовая MongoDB, д. {i}",
                TotalArea = 50.0 + i * 10
            };
            var propResponse = await _client.PostAsJsonAsync("/api/properties", property);
            var createdProp = await propResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
                RealEstateWebApplicationFactory.JsonOptions);

            var request = new CreateRequestDto
            {
                CounterpartyId = createdBuyer!.Id,
                PropertyId = createdProp!.Id,
                Type = RequestType.Purchase,
                Amount = 5000000.00m + i * 1000000,
                Date = DateTime.Now.AddDays(-i)
            };
            await _client.PostAsJsonAsync("/api/requests", request);
        }

        var response = await _client.GetAsync("/api/analytics/top-clients");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Top5ClientsResultDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(result);
        Assert.NotNull(result.TopPurchaseClients);

        var purchaseClientNames = result.TopPurchaseClients.Select(c => c.FullName).ToList();
        Assert.Contains("Активный покупатель MongoDB", purchaseClientNames);
    }
}
