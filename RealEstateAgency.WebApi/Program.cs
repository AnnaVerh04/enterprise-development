using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RealEstateAgency.Application.Mapping;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Contracts.Interfaces;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Infrastructure.Persistence;
using RealEstateAgency.Infrastructure.Repositories;
using RealEstateAgency.ServiceDefaults;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var useMongoDb = builder.Configuration.GetConnectionString("realestatedb") != null
    || Environment.GetEnvironmentVariable("ConnectionStrings__realestatedb") != null;

if (useMongoDb)
{
    builder.AddServiceDefaults();

    builder.AddMongoDBClient("realestatedb");

    builder.Services.AddDbContext<RealEstateDbContext>((serviceProvider, options) =>
    {
        var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        options.UseMongoDB(mongoClient, "realestatedb");
    });

    builder.Services.AddScoped<ICounterpartyRepository, MongoCounterpartyRepository>();
    builder.Services.AddScoped<IRealEstatePropertyRepository, MongoRealEstatePropertyRepository>();
    builder.Services.AddScoped<IRequestRepository, MongoRequestRepository>();
    builder.Services.AddScoped<DatabaseSeeder>();
}
else
{
    builder.Services.AddSingleton<ICounterpartyRepository, InMemoryCounterpartyRepository>();
    builder.Services.AddSingleton<IRealEstatePropertyRepository, InMemoryRealEstatePropertyRepository>();
    builder.Services.AddSingleton<IRequestRepository, InMemoryRequestRepository>();
}

builder.Services.AddScoped<ICounterpartyService, CounterpartyService>();
builder.Services.AddScoped<IRealEstatePropertyService, RealEstatePropertyService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Real Estate Agency API",
        Version = "v1",
        Description = "API риэлторского агентства для работы с объектами недвижимости, контрагентами и заявками"
    });

    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (useMongoDb)
{
    app.MapDefaultEndpoints();

    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate Agency API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
