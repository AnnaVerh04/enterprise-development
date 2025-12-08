using System.Text.Json.Serialization;
using RealEstateAgency.WebApi.Mapping;
using RealEstateAgency.WebApi.Repositories;
using RealEstateAgency.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрация репозиториев (In-Memory)
builder.Services.AddSingleton<ICounterpartyRepository, InMemoryCounterpartyRepository>();
builder.Services.AddSingleton<IRealEstatePropertyRepository, InMemoryRealEstatePropertyRepository>();
builder.Services.AddSingleton<IRequestRepository, InMemoryRequestRepository>();

// Регистрация сервиса аналитики
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Swagger/OpenAPI
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

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate Agency API v1");
        options.RoutePrefix = string.Empty;  
    });
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  

app.Run();

public partial class Program { }