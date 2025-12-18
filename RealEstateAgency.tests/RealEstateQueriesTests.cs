using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

/// <summary>
/// LINQ query tests for a real estate agency
/// </summary>
public class RealEstateQueriesTests(RealEstateTestFixture fixture) : IClassFixture<RealEstateTestFixture>
{
    /// <summary>
    /// The test for the request: "Withdraw all sellers who submitted applications for a specified period"
    /// </summary>
    [Fact]
    public void GetSellersInPeriodReturnsCorrectSellers()
    {
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 6, 30);

        List<string> expectedSellers = [
            "Зайцева Наталья Петровна",
            "Козлова Мария Владимировна",
            "Орлова Екатерина Дмитриевна",
            "Семенова Ольга Игоревна"
        ];

        var actualSellers = fixture.Requests
            .Where(r => r.Type == RequestType.Sale &&
                        r.Date >= startDate &&
                        r.Date <= endDate)
            .Select(r => r.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.NotNull(actualSellers);
        Assert.Equal(expectedSellers, actualSellers);
    }

    /// <summary>
    /// The test for the request: "Bring out the top 5 clients by the number of requests (separately for purchase and sale)"
    /// </summary>
    [Fact]
    public void Top5ClientsByRequestCountReturnsSeparateTop5()
    {
        List<string> expectedTopPurchaseClients = [
        "Сидоров Алексей Петрович",
        "Волков Павел Александрович",
        "Морозов Андрей Сергеевич",
        "Николаев Дмитрий Олегович",
        "Петрова Анна Сергеевна"
        ];

        List<string> expectedTopSaleClients = [
        "Козлова Мария Владимировна",
        "Белов Игорь Васильевич",
        "Зайцева Наталья Петровна",
        "Иванов Иван Иванович",
        "Орлова Екатерина Дмитриевна"
        ];

        var topPurchaseClients = fixture.Requests
            .Where(r => r.Type == RequestType.Purchase)
            .GroupBy(r => r.Counterparty)
            .Select(g => new { Counterparty = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Counterparty.FullName)
            .Take(5)
            .Select(x => x.Counterparty.FullName)
            .ToList();

        var topSaleClients = fixture.Requests
            .Where(r => r.Type == RequestType.Sale)
            .GroupBy(r => r.Counterparty)
            .Select(g => new { Counterparty = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Counterparty.FullName)
            .Take(5)
            .Select(x => x.Counterparty.FullName)
            .ToList();

        Assert.Equal(expectedTopPurchaseClients, topPurchaseClients);
        Assert.Equal(expectedTopSaleClients, topSaleClients);
    }

    /// <summary>
    /// The test for the request: "Display information on the number of applications for each type of property"
    /// </summary>
    [Fact]
    public void RequestCountByPropertyTypeReturnsCorrectStatistics()
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

        var actualStatistics = fixture.Requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new { PropertyType = g.Key, RequestCount = g.Count() })
            .OrderBy(x => x.PropertyType)
            .ToList();

        Assert.Equal(expectedStats.Count, actualStatistics.Count);

        foreach (var expected in expectedStats)
        {
            var actual = actualStatistics.First(x => x.PropertyType == expected.Key);
            Assert.Equal(expected.Value, actual.RequestCount);
        }
    }

    /// <summary>
    /// The test for the request: "Display information about clients who have opened applications with a minimum cost"
    /// </summary>
    [Fact]
    public void ClientsWithMinAmountAreFoundCorrectly()
    {
        var expectedMinAmount = 1500000.00m;
        List<string> expectedClients = ["Зайцева Наталья Петровна"];

        var minAmount = fixture.Requests.Min(r => r.Amount);
        var actualClients = fixture.Requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expectedMinAmount, minAmount);
        Assert.Equal(expectedClients, actualClients);
    }

    /// <summary>
    /// The test for the request: "Display information about all clients looking for a given type of property, sort by full name"
    /// </summary>
    [Fact]
    public void ClientsSeekingPropertyTypeAreReturnedOrdered()
    {
        var targetType = PropertyType.Apartment;
        List<string> expectedClients = [
            "Петрова Анна Сергеевна",
            "Сидоров Алексей Петрович"
        ];

        var actualClients = fixture.Requests
            .Where(r => r.Type == RequestType.Purchase &&
                        r.Property.Type == targetType)
            .Select(r => r.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expectedClients, actualClients);
    }
}