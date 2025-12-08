using System.Net;
using System.Net.Http.Json;
using RealEstateAgency.WebApi.DTOs;
using Xunit;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Тесты CRUD операций для контрагентов
/// </summary>
public class CounterpartiesControllerTests : IClassFixture<RealEstateWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CounterpartiesControllerTests(RealEstateWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// GET /api/counterparties — получение всех контрагентов
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsAllCounterparties()
    {
        var response = await _client.GetAsync("/api/counterparties");

        response.EnsureSuccessStatusCode();
        var counterparties = await response.Content.ReadFromJsonAsync<List<CounterpartyDto>>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(counterparties);
        Assert.True(counterparties.Count >= 12); 
    }

    /// <summary>
    /// GET /api/counterparties/{id} — получение контрагента по ID
    /// </summary>
    [Fact]
    public async Task GetById_ExistingId_ReturnsCounterparty()
    {
        var response = await _client.GetAsync("/api/counterparties/1");

        response.EnsureSuccessStatusCode();
        var counterparty = await response.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(counterparty);
        Assert.Equal(1, counterparty.Id);
        Assert.Equal("Иванов Иван Иванович", counterparty.FullName);
    }

    /// <summary>
    /// GET /api/counterparties/{id} — несуществующий ID возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/counterparties/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// POST /api/counterparties — создание нового контрагента
    /// </summary>
    [Fact]
    public async Task Create_ValidData_ReturnsCreatedCounterparty()
    {
        var newCounterparty = new CreateCounterpartyDto
        {
            FullName = "Тестов Тест Тестович",
            PassportNumber = "1234 567890",
            PhoneNumber = "+7-999-000-00-00"
        };

        var response = await _client.PostAsJsonAsync("/api/counterparties", newCounterparty);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("Тестов Тест Тестович", created.FullName);
    }

    /// <summary>
    /// PUT /api/counterparties/{id} — обновление контрагента
    /// </summary>
    [Fact]
    public async Task Update_ExistingId_ReturnsUpdatedCounterparty()
    {
        var updateData = new UpdateCounterpartyDto
        {
            FullName = "Иванов Иван Петрович",
            PassportNumber = "4501 123456",
            PhoneNumber = "+7-999-111-22-33"
        };

        var response = await _client.PutAsJsonAsync("/api/counterparties/1", updateData);

        response.EnsureSuccessStatusCode();
        var updated = await response.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        Assert.NotNull(updated);
        Assert.Equal("Иванов Иван Петрович", updated.FullName);
    }

    /// <summary>
    /// DELETE /api/counterparties/{id} — удаление контрагента
    /// </summary>
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var newCounterparty = new CreateCounterpartyDto
        {
            FullName = "Удаляемый Клиент",
            PassportNumber = "0000 000000",
            PhoneNumber = "+7-000-000-00-00"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/counterparties", newCounterparty);
        var created = await createResponse.Content.ReadFromJsonAsync<CounterpartyDto>(
            RealEstateWebApplicationFactory.JsonOptions);

        var response = await _client.DeleteAsync($"/api/counterparties/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/counterparties/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
