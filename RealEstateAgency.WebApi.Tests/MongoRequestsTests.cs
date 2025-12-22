using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Интеграционные тесты CRUD операций для заявок с реальной MongoDB
/// </summary>
[Collection("MongoDB")]
public class MongoRequestsTests(MongoDbWebApplicationFactory factory) : IClassFixture<MongoDbWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    /// <summary>
    /// Полный CRUD цикл для заявки в MongoDB
    /// </summary>
    [Fact]
    public async Task Request_FullCrudCycle_WorksWithMongoDB()
    {
        var counterparty = new CreateCounterpartyDto
        {
            FullName = "Клиент для заявки MongoDB",
            PassportNumber = "1111 222222",
            PhoneNumber = "+7-111-222-33-44"
        };
        var counterpartyResponse = await _client.PostAsJsonAsync("/api/counterparties", counterparty);
        var createdCounterparty = await counterpartyResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var property = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:11:1111111:111",
            Address = "ул. Заявочная, д. 1",
            TotalArea = 50.0
        };
        var propertyResponse = await _client.PostAsJsonAsync("/api/properties", property);
        var createdProperty = await propertyResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var newRequest = new CreateRequestDto
        {
            CounterpartyId = createdCounterparty!.Id,
            PropertyId = createdProperty!.Id,
            Type = RequestType.Sale,
            Amount = 5000000.00m,
            Date = new DateTime(2024, 6, 15)
        };

        var createResponse = await _client.PostAsJsonAsync("/api/requests", newRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal(5000000.00m, created.Amount);
        Assert.Equal(RequestType.Sale, created.Type);

        var createdId = created.Id;

        var getResponse = await _client.GetAsync($"/api/requests/{createdId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(fetched);
        Assert.Equal(createdId, fetched.Id);
        Assert.Equal(5000000.00m, fetched.Amount);

        var updateData = new UpdateRequestDto
        {
            CounterpartyId = createdCounterparty.Id,
            PropertyId = createdProperty.Id,
            Type = RequestType.Sale,
            Amount = 5500000.00m,
            Date = new DateTime(2024, 6, 20)
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/requests/{createdId}", updateData);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(updated);
        Assert.Equal(5500000.00m, updated.Amount);

        var deleteResponse = await _client.DeleteAsync($"/api/requests/{createdId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getDeletedResponse = await _client.GetAsync($"/api/requests/{createdId}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }

    /// <summary>
    /// Тест создания заявок разных типов в MongoDB
    /// </summary>
    [Theory]
    [InlineData(RequestType.Sale)]
    [InlineData(RequestType.Purchase)]
    public async Task Create_BothRequestTypes_SavedCorrectlyInMongoDB(RequestType requestType)
    {
        var counterparty = new CreateCounterpartyDto
        {
            FullName = $"Клиент для {requestType}",
            PassportNumber = $"000{(int)requestType} 000000",
            PhoneNumber = $"+7-000-000-00-0{(int)requestType}"
        };
        var counterpartyResponse = await _client.PostAsJsonAsync("/api/counterparties", counterparty);
        var createdCounterparty = await counterpartyResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var property = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.House,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = $"77:22:222222{(int)requestType}:222",
            Address = $"Адрес для {requestType}",
            TotalArea = 150.0
        };
        var propertyResponse = await _client.PostAsJsonAsync("/api/properties", property);
        var createdProperty = await propertyResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var request = new CreateRequestDto
        {
            CounterpartyId = createdCounterparty!.Id,
            PropertyId = createdProperty!.Id,
            Type = requestType,
            Amount = 10000000.00m,
            Date = DateTime.Now
        };

        var response = await _client.PostAsJsonAsync("/api/requests", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<RequestDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(created);
        Assert.Equal(requestType, created.Type);
    }

    /// <summary>
    /// Тест валидации - несуществующий контрагент
    /// </summary>
    [Fact]
    public async Task Create_NonExistingCounterparty_ReturnsNotFound()
    {
        var property = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:33:3333333:333",
            Address = "Адрес без контрагента",
            TotalArea = 40.0
        };
        var propertyResponse = await _client.PostAsJsonAsync("/api/properties", property);
        var createdProperty = await propertyResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var request = new CreateRequestDto
        {
            CounterpartyId = Guid.NewGuid(),
            PropertyId = createdProperty!.Id,
            Type = RequestType.Sale,
            Amount = 1000000.00m,
            Date = DateTime.Now
        };

        var response = await _client.PostAsJsonAsync("/api/requests", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
