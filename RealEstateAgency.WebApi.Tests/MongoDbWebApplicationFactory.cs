using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RealEstateAgency.WebApi.Repositories;
using Testcontainers.MongoDb;
using Xunit;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Фабрика для интеграционных тестов с реальной MongoDB (через Testcontainers)
/// </summary>
public class MongoDbWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:7.0")
        .Build();

    public string ConnectionString => _mongoDbContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d => d.ServiceType == typeof(ICounterpartyRepository) ||
                            d.ServiceType == typeof(IRealEstatePropertyRepository) ||
                            d.ServiceType == typeof(IRequestRepository) ||
                            d.ServiceType == typeof(IMongoClient) ||
                            d.ServiceType == typeof(IMongoDatabase))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            var mongoClient = new MongoClient(ConnectionString);
            var database = mongoClient.GetDatabase("realestatedb_test");

            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(database);

            services.AddSingleton<ICounterpartyRepository, MongoCounterpartyRepository>();
            services.AddSingleton<IRealEstatePropertyRepository, MongoRealEstatePropertyRepository>();
            services.AddSingleton<IRequestRepository, MongoRequestRepository>();
        });

        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
    }
}
