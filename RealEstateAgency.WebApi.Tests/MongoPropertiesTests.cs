using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Интеграционные тесты CRUD операций для объектов недвижимости с реальной MongoDB
/// </summary>
[Collection("MongoDB")]
public class MongoPropertiesTests : IClassFixture<MongoDbWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MongoPropertiesTests(MongoDbWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Полный CRUD цикл для объекта недвижимости в MongoDB
    /// </summary>
    [Fact]
    public async Task Property_FullCrudCycle_WorksWithMongoDB()
    {
        var newProperty = new CreateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:00:0000000:001",
            Address = "ул. MongoDB, д. 1, кв. 1",
            TotalFloors = 10,
            TotalArea = 65.5,
            RoomsCount = 2,
            CeilingHeight = 2.8,
            Floor = 5,
            HasEncumbrances = false
        };

        var createResponse = await _client.PostAsJsonAsync("/api/properties", newProperty);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("ул. MongoDB, д. 1, кв. 1", created.Address);
        Assert.Equal(PropertyType.Apartment, created.Type);

        var createdId = created.Id;

        var getResponse = await _client.GetAsync($"/api/properties/{createdId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(fetched);
        Assert.Equal(createdId, fetched.Id);
        Assert.Equal(65.5, fetched.TotalArea);

        var updateData = new UpdateRealEstatePropertyDto
        {
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:00:0000000:001",
            Address = "ул. MongoDB, д. 1, кв. 1 (обновлено)",
            TotalFloors = 10,
            TotalArea = 70.0,
            RoomsCount = 3,
            CeilingHeight = 2.8,
            Floor = 5,
            HasEncumbrances = false
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/properties/{createdId}", updateData);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(updated);
        Assert.Equal(70.0, updated.TotalArea);
        Assert.Equal(3, updated.RoomsCount);

        var deleteResponse = await _client.DeleteAsync($"/api/properties/{createdId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getDeletedResponse = await _client.GetAsync($"/api/properties/{createdId}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }

    /// <summary>
    /// Тест сохранения всех типов недвижимости в MongoDB
    /// </summary>
    [Theory]
    [InlineData(PropertyType.Apartment, PropertyPurpose.Residential)]
    [InlineData(PropertyType.House, PropertyPurpose.Residential)]
    [InlineData(PropertyType.Townhouse, PropertyPurpose.Residential)]
    [InlineData(PropertyType.Commercial, PropertyPurpose.Commercial)]
    [InlineData(PropertyType.Warehouse, PropertyPurpose.Industrial)]
    [InlineData(PropertyType.ParkingSpace, PropertyPurpose.Commercial)]
    public async Task Create_AllPropertyTypes_SavedCorrectlyInMongoDB(
        PropertyType type, PropertyPurpose purpose)
    {
        var property = new CreateRealEstatePropertyDto
        {
            Type = type,
            Purpose = purpose,
            CadastralNumber = $"77:00:000000{(int)type}:{(int)purpose}",
            Address = $"Тестовый адрес для {type}",
            TotalArea = 100.0
        };

        var response = await _client.PostAsJsonAsync("/api/properties", property);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<RealEstatePropertyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(created);
        Assert.Equal(type, created.Type);
        Assert.Equal(purpose, created.Purpose);
    }
}
