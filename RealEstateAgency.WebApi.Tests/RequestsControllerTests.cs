using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Тесты CRUD операций для заявок
/// </summary>
public class RequestsControllerTests(RealEstateWebApplicationFactory factory)
    : IClassFixture<RealEstateWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static readonly Guid _testRequestId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid _testCounterpartyId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid _testPropertyId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    /// <summary>
    /// GET /api/requests — получение всех заявок
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsAllRequests()
    {
        var response = await _client.GetAsync("/api/requests");

        response.EnsureSuccessStatusCode();
        var requests = await response.Content.ReadFromJsonAsync<List<RequestDto>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(requests);
        Assert.True(requests.Count >= 15);
    }

    /// <summary>
    /// GET /api/requests/{id} — получение заявки по ID
    /// </summary>
    [Fact]
    public async Task GetById_ExistingId_ReturnsRequest()
    {
        var response = await _client.GetAsync($"/api/requests/{_testRequestId}");

        response.EnsureSuccessStatusCode();
        var request = await response.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(request);
        Assert.Equal(_testRequestId, request.Id);
        Assert.Equal(RequestType.Sale, request.Type);
        Assert.Equal(25000000.00m, request.Amount);
    }

    /// <summary>
    /// GET /api/requests/{id} — несуществующий ID возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/requests/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// POST /api/requests — создание новой заявки
    /// </summary>
    [Fact]
    public async Task Create_ValidData_ReturnsCreatedRequest()
    {
        var newRequest = new CreateRequestDto
        {
            CounterpartyId = _testCounterpartyId,
            PropertyId = _testPropertyId,
            Type = RequestType.Purchase,
            Amount = 30000000.00m,
            Date = new DateTime(2024, 10, 15)
        };

        var response = await _client.PostAsJsonAsync("/api/requests", newRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal(30000000.00m, created.Amount);
    }

    /// <summary>
    /// POST /api/requests — несуществующий контрагент возвращает 404
    /// </summary>
    [Fact]
    public async Task Create_InvalidCounterpartyId_ReturnsNotFound()
    {
        var newRequest = new CreateRequestDto
        {
            CounterpartyId = Guid.NewGuid(),
            PropertyId = _testPropertyId,
            Type = RequestType.Sale,
            Amount = 1000000.00m,
            Date = DateTime.Now
        };

        var response = await _client.PostAsJsonAsync("/api/requests", newRequest);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// PUT /api/requests/{id} — обновление заявки
    /// </summary>
    [Fact]
    public async Task Update_ExistingId_ReturnsUpdatedRequest()
    {
        var updateData = new UpdateRequestDto
        {
            CounterpartyId = _testCounterpartyId,
            PropertyId = _testPropertyId,
            Type = RequestType.Sale,
            Amount = 26000000.00m,
            Date = new DateTime(2024, 1, 15)
        };

        var response = await _client.PutAsJsonAsync($"/api/requests/{_testRequestId}", updateData);

        response.EnsureSuccessStatusCode();
        var updated = await response.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(updated);
        Assert.Equal(26000000.00m, updated.Amount);
    }

    /// <summary>
    /// DELETE /api/requests/{id} — удаление заявки
    /// </summary>
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var newRequest = new CreateRequestDto
        {
            CounterpartyId = _testCounterpartyId,
            PropertyId = _testPropertyId,
            Type = RequestType.Purchase,
            Amount = 100.00m,
            Date = DateTime.Now
        };
        var createResponse = await _client.PostAsJsonAsync("/api/requests", newRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var response = await _client.DeleteAsync($"/api/requests/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
