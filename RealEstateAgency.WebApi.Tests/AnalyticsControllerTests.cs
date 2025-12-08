using System.Net.Http.Json;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.WebApi.DTOs;
using Xunit;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Тесты аналитических запросов
/// Проверяют соответствие результатов API ожидаемым значениям из Unit-тестов первой лабораторной
/// </summary>
public class AnalyticsControllerTests : IClassFixture<RealEstateWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AnalyticsControllerTests(RealEstateWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Тест запроса: "Вывести всех продавцов, подавших заявки за указанный период"
    /// Ожидаемый результат из RealEstateQueriesTests.GetSellersInPeriodReturnsCorrectSellers()
    /// </summary>
    [Fact]
    public async Task GetSellersInPeriod_ReturnsCorrectSellers()
    {
        var startDate = "2024-03-01";
        var endDate = "2024-06-30";
        List<string> expectedSellers =
        [
            "Зайцева Наталья Петровна",
            "Козлова Мария Владимировна",
            "Орлова Екатерина Дмитриевна",
            "Семенова Ольга Игоревна"
        ];

        var response = await _client.GetAsync(
            $"/api/analytics/sellers?startDate={startDate}&endDate={endDate}");

        response.EnsureSuccessStatusCode();
        var actualSellers = await response.Content.ReadFromJsonAsync<List<string>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(actualSellers);
        Assert.Equal(expectedSellers, actualSellers);
    }

    /// <summary>
    /// Тест запроса: "Вывести топ-5 клиентов по количеству заявок (отдельно покупка/продажа)"
    /// Ожидаемый результат из RealEstateQueriesTests.Top5ClientsByRequestCountReturnsSeparateTop5()
    /// </summary>
    [Fact]
    public async Task GetTop5Clients_ReturnsCorrectTop5()
    {
        List<string> expectedTopPurchaseClients =
        [
            "Сидоров Алексей Петрович",
            "Волков Павел Александрович",
            "Морозов Андрей Сергеевич",
            "Николаев Дмитрий Олегович",
            "Петрова Анна Сергеевна"
        ];

        List<string> expectedTopSaleClients =
        [
            "Козлова Мария Владимировна",
            "Белов Игорь Васильевич",
            "Зайцева Наталья Петровна",
            "Иванов Иван Иванович",
            "Орлова Екатерина Дмитриевна"
        ];

        var response = await _client.GetAsync("/api/analytics/top-clients");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Top5ClientsResultDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(result);

        var actualPurchaseNames = result.TopPurchaseClients.Select(c => c.FullName).ToList();
        var actualSaleNames = result.TopSaleClients.Select(c => c.FullName).ToList();

        Assert.Equal(expectedTopPurchaseClients, actualPurchaseNames);
        Assert.Equal(expectedTopSaleClients, actualSaleNames);
    }

    /// <summary>
    /// Тест запроса: "Вывести информацию о количестве заявок по каждому типу недвижимости"
    /// Ожидаемый результат из RealEstateQueriesTests.RequestCountByPropertyTypeReturnsCorrectStatistics()
    /// </summary>
    [Fact]
    public async Task GetPropertyTypeStatistics_ReturnsCorrectStatistics()
    {
        var expectedStats = new Dictionary<PropertyType, int>
        {
            [PropertyType.Apartment] = 5,
            [PropertyType.House] = 2,
            [PropertyType.Townhouse] = 2,
            [PropertyType.Commercial] = 2,
            [PropertyType.ParkingSpace] = 2,
            [PropertyType.Warehouse] = 2
        };

        var response = await _client.GetAsync("/api/analytics/property-type-statistics");

        response.EnsureSuccessStatusCode();
        var actualStats = await response.Content.ReadFromJsonAsync<List<PropertyTypeStatisticsDto>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(actualStats);
        Assert.Equal(expectedStats.Count, actualStats.Count);

        foreach (var expected in expectedStats)
        {
            var actual = actualStats.First(x => x.PropertyType == expected.Key);
            Assert.Equal(expected.Value, actual.RequestCount);
        }
    }

    /// <summary>
    /// Тест запроса: "Вывести информацию о клиентах, открывших заявки с минимальной стоимостью"
    /// Ожидаемый результат из RealEstateQueriesTests.ClientsWithMinAmountAreFoundCorrectly()
    /// </summary>
    [Fact]
    public async Task GetClientsWithMinAmount_ReturnsCorrectClients()
    {
        var expectedMinAmount = 1500000.00m;
        var expectedClient = "Зайцева Наталья Петровна";

        var response = await _client.GetAsync("/api/analytics/min-amount-clients");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ClientWithMinAmountDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(expectedMinAmount, result.MinAmount);
        Assert.Equal(expectedClient, result.FullName);
    }

    /// <summary>
    /// Тест запроса: "Вывести информацию о всех клиентах, ищущих заданный тип недвижимости"
    /// Ожидаемый результат из RealEstateQueriesTests.ClientsSeekingPropertyTypeAreReturnedOrdered()
    /// </summary>
    [Fact]
    public async Task GetClientsByPropertyType_Apartment_ReturnsCorrectClients()
    {
        List<string> expectedClients =
        [
            "Петрова Анна Сергеевна",
            "Сидоров Алексей Петрович"
        ];

        var response = await _client.GetAsync(
            "/api/analytics/clients-by-property-type?propertyType=Apartment");

        response.EnsureSuccessStatusCode();
        var actualClients = await response.Content.ReadFromJsonAsync<List<string>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(actualClients);
        Assert.Equal(expectedClients, actualClients);
    }
}
