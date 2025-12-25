using RealEstateAgency.Generator.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var natsConnectionString = builder.Configuration["ConnectionStrings:nats"]
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__nats")
    ?? "nats://localhost:4222";

var batchSize = int.TryParse(
    builder.Configuration["Generator:BatchSize"] ??
    Environment.GetEnvironmentVariable("GENERATOR_BATCH_SIZE"),
    out var bs) ? bs : 10;

var delayMs = int.TryParse(
    builder.Configuration["Generator:DelayMs"] ??
    Environment.GetEnvironmentVariable("GENERATOR_DELAY_MS"),
    out var dm) ? dm : 5000;

builder.Services.AddSingleton(sp =>
    new NatsPublisher(
        natsConnectionString,
        sp.GetRequiredService<ILogger<NatsPublisher>>()));

builder.Services.AddHostedService(sp =>
    new DataGeneratorService(
        sp.GetRequiredService<NatsPublisher>(),
        sp.GetRequiredService<ILogger<DataGeneratorService>>(),
        batchSize,
        delayMs));

var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== RealEstateAgency Data Generator ===");
logger.LogInformation("NATS Connection: {Connection}", natsConnectionString);
logger.LogInformation("Batch Size: {BatchSize}", batchSize);
logger.LogInformation("Delay between batches: {Delay}ms", delayMs);

await host.RunAsync();
