using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Runtime.CompilerServices;

namespace RealEstateAgency.WebApi.Tests;

/// <summary>
/// Инициализатор модуля - выполняется при загрузке сборки
/// </summary>
file static class MongoDbInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
#pragma warning disable CS0618 
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
        try
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }
        catch (BsonSerializationException)
        {
            // Сериализатор уже зарегистрирован
        }
    }
}

/// <summary>
/// Определение коллекции для MongoDB тестов
/// Все тесты в этой коллекции будут использовать один экземпляр MongoDbWebApplicationFactory
/// </summary>
[CollectionDefinition("MongoDB")]
public class MongoDbCollection : ICollectionFixture<MongoDbWebApplicationFactory>;
