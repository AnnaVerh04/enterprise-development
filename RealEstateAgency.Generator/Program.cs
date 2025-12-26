using Microsoft.Extensions.Options;
using RealEstateAgency.Generator.Services;
using RealEstateAgency.ServiceDefaults; 

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var natsConnectionString = "nats://localhost:4222";

builder.Services.Configure<DataGeneratorSettings>(
    builder.Configuration.GetSection(DataGeneratorSettings.SectionName));

builder.Services.AddSingleton(sp =>
    new NatsPublisher(
        natsConnectionString,
        sp.GetRequiredService<ILogger<NatsPublisher>>()));

builder.Services.AddHostedService<DataGeneratorService>();

var host = builder.Build();

var settings = host.Services.GetRequiredService<IOptions<DataGeneratorSettings>>().Value;
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("=== RealEstateAgency Data Generator ===");
logger.LogInformation("NATS Connection: {Connection}", natsConnectionString);
logger.LogInformation("Batch Size: {BatchSize}", settings.BatchSize);
logger.LogInformation("Delay between batches: {Delay}ms", settings.DelayBetweenBatchesMs);

await host.RunAsync();