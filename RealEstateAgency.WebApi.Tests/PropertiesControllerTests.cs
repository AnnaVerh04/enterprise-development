using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Тесты CRUD операций для объектов недвижимости
/// </summary>
public class PropertiesControllerTests(RealEstateWebApplicationFactory factory)
    : IClassFixture<RealEstateWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static readonly Guid _testPropertyId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    /// <summary>
    /// GET /api/properties — получение всех объектов
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsAllProperties()
    {
        var response = await _client.GetAsync("/api/properties");

        response.EnsureSuccessStatusCode();
        var properties = await response.Content.ReadFromJsonAsync<List<RealEstatePropertyDto>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(properties);
        Assert.True(properties.Count >= 13);
    }

    /// <summary>
    /// GET /api/properties/{id} — получение объекта по ID
    /// </summary>
    [Fact]
    public async Task GetById_ExistingId_ReturnsProperty()
    {
        var response = await _client.GetAsync($"/api/properties/{_testPropertyId}");

        response.EnsureSuccessStatusCode();
        var property = await response.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(property);
        Assert.Equal(_testPropertyId, property.Id);
        Assert.Equal(PropertyType.Apartment, property.Type);
        Assert.Contains("ул. Тверская, 15, кв. 34", property.Address);
    }

    /// <summary>
    /// GET /api/properties/{id} — несуществующий ID возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/properties/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// POST /api/properties — создание нового объекта
    /// </summary>
    [Fact]
    public async Task Create_ValidData_ReturnsCreatedProperty()
    {
        var newProperty = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:99:0009999:999",
            Address = "ул. Тестовая, 1, кв. 1",
            TotalFloors = 10,
            TotalArea = 50.0,
            RoomsCount = 2,
            CeilingHeight = 2.7,
            Floor = 5,
            HasEncumbrances = false
        };

        var response = await _client.PostAsJsonAsync("/api/properties", newProperty);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("ул. Тестовая, 1, кв. 1", created.Address);
    }

    /// <summary>
    /// PUT /api/properties/{id} — обновление объекта
    /// </summary>
    [Fact]
    public async Task Update_ExistingId_ReturnsUpdatedProperty()
    {
        var updateData = new UpdateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:01:0001001:101",
            Address = "ул. Тверская, 15, кв. 34 (обновлено)",
            TotalFloors = 9,
            TotalArea = 80.0,
            RoomsCount = 3,
            CeilingHeight = 2.7,
            Floor = 5,
            HasEncumbrances = false
        };

        var response = await _client.PutAsJsonAsync($"/api/properties/{_testPropertyId}", updateData);

        response.EnsureSuccessStatusCode();
        var updated = await response.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(updated);
        Assert.Equal(80.0, updated.TotalArea);
    }

    /// <summary>
    /// DELETE /api/properties/{id} — удаление объекта
    /// </summary>
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var newProperty = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.ParkingSpace,
            Purpose = PropertyPurpose.Commercial,
            CadastralNumber = "00:00:0000000:000",
            Address = "Удаляемый объект",
            TotalArea = 10.0
        };
        var createResponse = await _client.PostAsJsonAsync("/api/properties", newProperty);
        var created = await createResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var response = await _client.DeleteAsync($"/api/properties/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
