using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

public class RealEstateQueriesTests
{
    private readonly RealEstateTestFixture _fixture;

    public RealEstateQueriesTests()
    {
        _fixture = new RealEstateTestFixture();
    }

    [Fact]
    public void GetSellersInPeriod_ReturnsCorrectSellers()
    {
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 6, 30);

        var expectedSellers = _fixture.Requests
            .Where(r => r.Type == RequestType.Sale &&
                        r.Date >= startDate &&
                        r.Date <= endDate)
            .Select(r => r.Counterparty)
            .Distinct()
            .ToList();

        var actualSellers = _fixture.Requests
            .Where(r => r.Type == RequestType.Sale &&
                        r.Date >= startDate &&
                        r.Date <= endDate)
            .Select(r => r.Counterparty)
            .Distinct()
            .ToList();

        Assert.NotNull(actualSellers);
        Assert.Equal(expectedSellers.Count, actualSellers.Count);
    }

    [Fact]
    public void Top5ClientsByRequestCount_ReturnsSeparateTop5()
    {
        var topPurchaseClients = _fixture.Requests
            .Where(r => r.Type == RequestType.Purchase)
            .GroupBy(r => r.Counterparty)
            .Select(g => new
            {
                Counterparty = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Counterparty.FullName)
            .Take(5)
            .ToList();

        var topSaleClients = _fixture.Requests
            .Where(r => r.Type == RequestType.Sale)
            .GroupBy(r => r.Counterparty)
            .Select(g => new
            {
                Counterparty = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Counterparty.FullName)
            .Take(5)
            .ToList();

        Assert.NotNull(topPurchaseClients);
        Assert.NotNull(topSaleClients);
        Assert.True(topPurchaseClients.Count <= 5);
        Assert.True(topSaleClients.Count <= 5);
    }

    [Fact]
    public void RequestCountByPropertyType_ReturnsStatistics()
    {
        var expectedStatistics = _fixture.Requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new
            {
                PropertyType = g.Key,
                RequestCount = g.Count()
            })
            .OrderBy(x => x.PropertyType)
            .ToList();

        var actualStatistics = _fixture.Requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new
            {
                PropertyType = g.Key,
                RequestCount = g.Count()
            })
            .OrderBy(x => x.PropertyType)
            .ToList();

        Assert.NotNull(actualStatistics);
        Assert.Equal(expectedStatistics.Count, actualStatistics.Count);
    }

    [Fact]
    public void ClientsWithMinAmount_AreFoundCorrectly()
    {
        var expectedMinAmount = _fixture.Requests.Min(r => r.Amount);

        var expectedClients = _fixture.Requests
            .Where(r => r.Amount == expectedMinAmount)
            .Select(r => r.Counterparty)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        var minAmount = _fixture.Requests.Min(r => r.Amount);
        var actualClients = _fixture.Requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Counterparty)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();


        Assert.NotNull(actualClients);
        Assert.Equal(expectedMinAmount, minAmount);
        Assert.Equal(expectedClients.Count, actualClients.Count);
    }

    [Fact]
    public void ClientsSeekingPropertyType_AreReturnedOrdered()
    {
        var targetType = PropertyType.Apartment;

        var expectedClients = _fixture.Requests
            .Where(r => r.Type == RequestType.Purchase &&
                        r.Property.Type == targetType)
            .Select(r => r.Counterparty)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        var actualClients = _fixture.Requests
            .Where(r => r.Type == RequestType.Purchase &&
                        r.Property.Type == targetType)
            .Select(r => r.Counterparty)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();


        Assert.NotNull(actualClients);
        Assert.Equal(expectedClients.Count, actualClients.Count);
    }
}
