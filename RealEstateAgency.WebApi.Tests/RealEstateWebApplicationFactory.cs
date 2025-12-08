using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RealEstateAgency.WebApi.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Фабрика для создания тестового сервера
/// Использует in-memory репозитории для изоляции тестов
/// </summary>
public class RealEstateWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// Настройки JSON для десериализации ответов с enum как строками
    /// </summary>
    public static JsonSerializerOptions JsonOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d => d.ServiceType == typeof(ICounterpartyRepository) ||
                            d.ServiceType == typeof(IRealEstatePropertyRepository) ||
                            d.ServiceType == typeof(IRequestRepository))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<ICounterpartyRepository, InMemoryCounterpartyRepository>();
            services.AddSingleton<IRealEstatePropertyRepository, InMemoryRealEstatePropertyRepository>();
            services.AddSingleton<IRequestRepository, InMemoryRequestRepository>();
        });

        builder.UseEnvironment("Testing");
    }
}
