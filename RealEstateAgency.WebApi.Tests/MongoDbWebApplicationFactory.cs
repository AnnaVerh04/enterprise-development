using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Infrastructure.Persistence;
using RealEstateAgency.Infrastructure.Repositories;
using Testcontainers.MongoDb;

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
                            d.ServiceType == typeof(IMongoDatabase) ||
                            d.ServiceType == typeof(RealEstateDbContext) ||
                            d.ServiceType == typeof(DbContextOptions<RealEstateDbContext>))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            var mongoClient = new MongoClient(ConnectionString);
            services.AddSingleton<IMongoClient>(mongoClient);

            services.AddDbContext<RealEstateDbContext>(options =>
            {
                options.UseMongoDB(mongoClient, "realestatedb_test");
            });

            services.AddScoped<ICounterpartyRepository, MongoCounterpartyRepository>();
            services.AddScoped<IRealEstatePropertyRepository, MongoRealEstatePropertyRepository>();
            services.AddScoped<IRequestRepository, MongoRequestRepository>();
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
