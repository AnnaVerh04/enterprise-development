using RealEstateAgency.Contracts.Dto;
using System.Net;
using System.Net.Http.Json;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Интеграционные тесты CRUD операций для контрагентов с реальной MongoDB
/// </summary>
[Collection("MongoDB")]
public class MongoCounterpartiesTests : IClassFixture<MongoDbWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MongoCounterpartiesTests(MongoDbWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Полный CRUD цикл для контрагента в MongoDB
    /// </summary>
    [Fact]
    public async Task Counterparty_FullCrudCycle_WorksWithMongoDB()
    {
        var newCounterparty = new CreateCounterpartyDto
        {
            FullName = "Тестов Тест Тестович (MongoDB)",
            PassportNumber = "9999 888777",
            PhoneNumber = "+7-999-888-77-66"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/counterparties", newCounterparty);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Тестов Тест Тестович (MongoDB)", created.FullName);

        var createdId = created.Id;

        var getResponse = await _client.GetAsync($"/api/counterparties/{createdId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(fetched);
        Assert.Equal(createdId, fetched.Id);
        Assert.Equal("Тестов Тест Тестович (MongoDB)", fetched.FullName);

        var updateData = new UpdateCounterpartyDto
        {
            FullName = "Обновлённый Тест (MongoDB)",
            PassportNumber = "9999 888777",
            PhoneNumber = "+7-999-888-77-55"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/counterparties/{createdId}", updateData);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);
        Assert.NotNull(updated);
        Assert.Equal("Обновлённый Тест (MongoDB)", updated.FullName);

        var deleteResponse = await _client.DeleteAsync($"/api/counterparties/{createdId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getDeletedResponse = await _client.GetAsync($"/api/counterparties/{createdId}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }

    /// <summary>
    /// Получение списка контрагентов из MongoDB
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsListFromMongoDB()
    {
        for (var i = 1; i <= 3; i++)
        {
            var counterparty = new CreateCounterpartyDto
            {
                FullName = $"Контрагент MongoDB #{i}",
                PassportNumber = $"000{i} 00000{i}",
                PhoneNumber = $"+7-000-000-00-0{i}"
            };
            await _client.PostAsJsonAsync("/api/counterparties", counterparty);
        }

        var response = await _client.GetAsync("/api/counterparties");

        response.EnsureSuccessStatusCode();
        var counterparties = await response.Content.ReadFromJsonAsync<List<CounterpartyDto>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(counterparties);
        Assert.True(counterparties.Count >= 3);
    }
}
