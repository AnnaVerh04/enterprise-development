using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RealEstateAgency.ServiceDefaults;
using RealEstateAgency.WebApi.Mapping;
using RealEstateAgency.WebApi.Repositories;
using RealEstateAgency.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

var useMongoDB = builder.Configuration.GetConnectionString("realestatedb") != null
    || Environment.GetEnvironmentVariable("ConnectionStrings__realestatedb") != null;

if (useMongoDB)
{
    builder.AddServiceDefaults();

    BsonSerializer.RegisterSerializer(new EnumSerializer<RealEstateAgency.Domain.Enums.PropertyType>(BsonType.String));
    BsonSerializer.RegisterSerializer(new EnumSerializer<RealEstateAgency.Domain.Enums.PropertyPurpose>(BsonType.String));
    BsonSerializer.RegisterSerializer(new EnumSerializer<RealEstateAgency.Domain.Enums.RequestType>(BsonType.String));

    builder.AddMongoDBClient("realestatedb");

    builder.Services.AddScoped<IMongoDatabase>(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        return client.GetDatabase("realestatedb");
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

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

if (useMongoDB)
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
