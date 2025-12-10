using Xunit;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Определение коллекции для MongoDB тестов
/// Все тесты в этой коллекции будут использовать один экземпляр MongoDbWebApplicationFactory
/// </summary>
[CollectionDefinition("MongoDB")]
public class MongoDbCollection : ICollectionFixture<MongoDbWebApplicationFactory>
{
}
