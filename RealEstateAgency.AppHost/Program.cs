var builder = DistributedApplication.CreateBuilder(args);

// MongoDB
var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume("mongodb-data");

var mongoDatabase = mongodb.AddDatabase("realestatedb");

// NATS 
var nats = builder.AddNats("nats")
    .WithJetStream()
    .WithDataVolume("nats-data");

// WebApi
builder.AddProject<Projects.RealEstateAgency_WebApi>("webapi")
    .WithReference(mongoDatabase)
    .WithReference(nats)
    .WaitFor(mongoDatabase)
    .WaitFor(nats)
    .WithExternalHttpEndpoints();

// Generator 
builder.AddProject<Projects.RealEstateAgency_Generator>("generator")
    .WithReference(nats)
    .WaitFor(nats)
    .WaitFor(mongoDatabase);

builder.Build().Run();