var builder = DistributedApplication.CreateBuilder(args);

// MongoDB
var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume("mongodb-data");

var mongoDatabase = mongodb.AddDatabase("realestatedb");

// WebApi
builder.AddProject<Projects.RealEstateAgency_WebApi>("webapi")
    .WithReference(mongoDatabase)
    .WithExternalHttpEndpoints();

builder.Build().Run();