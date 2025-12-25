using Bogus;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Generator.Generators;

/// <summary>
/// Генератор контрактов недвижимости с использованием Bogus
/// </summary>
public static class ContractGenerator
{
    private static readonly string[] _russianFirstNames =
    [
        "Александр", "Дмитрий", "Максим", "Сергей", "Андрей", "Алексей", "Артём", "Илья", "Кирилл", "Михаил",
        "Никита", "Матвей", "Роман", "Егор", "Арсений", "Иван", "Денис", "Евгений", "Даниил", "Тимофей",
        "Анна", "Мария", "Елена", "Дарья", "Алина", "Ирина", "Екатерина", "Ольга", "Татьяна", "Наталья",
        "Юлия", "Виктория", "Марина", "Светлана", "Анастасия", "Полина", "Софья", "Валерия", "Ксения", "Вера"
    ];

    private static readonly string[] _russianLastNames =
    [
        "Иванов", "Смирнов", "Кузнецов", "Попов", "Васильев", "Петров", "Соколов", "Михайлов", "Новиков", "Федоров",
        "Морозов", "Волков", "Алексеев", "Лебедев", "Семёнов", "Егоров", "Павлов", "Козлов", "Степанов", "Николаев",
        "Орлов", "Андреев", "Макаров", "Никитин", "Захаров", "Зайцев", "Соловьёв", "Борисов", "Яковлев", "Григорьев"
    ];

    private static readonly string[] _russianPatronymics =
    [
        "Александрович", "Дмитриевич", "Сергеевич", "Андреевич", "Алексеевич", "Михайлович", "Владимирович", "Николаевич",
        "Александровна", "Дмитриевна", "Сергеевна", "Андреевна", "Алексеевна", "Михайловна", "Владимировна", "Николаевна"
    ];

    private static readonly string[] _russianCities =
    [
        "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань",
        "Нижний Новгород", "Челябинск", "Самара", "Омск", "Ростов-на-Дону"
    ];

    private static readonly string[] _russianStreets =
    [
        "Ленина", "Мира", "Советская", "Гагарина", "Пушкина", "Комсомольская",
        "Центральная", "Победы", "Октябрьская", "Молодёжная", "Садовая", "Парковая"
    ];

    private static readonly Faker _faker = new("ru");

    /// <summary>
    /// Генерирует DTO контрагента
    /// </summary>
    public static CreateCounterpartyDto GenerateCounterparty()
    {
        var firstName = _faker.PickRandom(_russianFirstNames);
        var lastName = _faker.PickRandom(_russianLastNames);
        var patronymic = _faker.PickRandom(_russianPatronymics);

        return new CreateCounterpartyDto
        {
            FullName = $"{lastName} {firstName} {patronymic}",
            PassportNumber = $"{_faker.Random.Number(1000, 9999)} {_faker.Random.Number(100000, 999999)}",
            PhoneNumber = $"+7{_faker.Random.Number(900, 999)}{_faker.Random.Number(1000000, 9999999)}"
        };
    }

    /// <summary>
    /// Генерирует DTO объекта недвижимости
    /// </summary>
    public static CreateRealEstatePropertyDto GenerateProperty()
    {
        var propertyType = _faker.PickRandom<PropertyType>();
        var purpose = propertyType switch
        {
            PropertyType.Apartment or PropertyType.House or PropertyType.Townhouse => PropertyPurpose.Residential,
            PropertyType.Commercial => PropertyPurpose.Commercial,
            PropertyType.Warehouse => PropertyPurpose.Industrial,
            PropertyType.ParkingSpace => _faker.PickRandom<PropertyPurpose>(),
            _ => PropertyPurpose.Residential
        };

        var city = _faker.PickRandom(_russianCities);
        var street = _faker.PickRandom(_russianStreets);
        var houseNumber = _faker.Random.Number(1, 150);
        var apartment = propertyType == PropertyType.Apartment ? $", кв. {_faker.Random.Number(1, 500)}" : "";

        var cadastralNumber = $"{_faker.Random.Number(10, 99)}:{_faker.Random.Number(10, 99)}:" +
                              $"{_faker.Random.Number(1000000, 9999999)}:{_faker.Random.Number(100, 999)}";

        var totalFloors = propertyType switch
        {
            PropertyType.Apartment => _faker.Random.Number(5, 25),
            PropertyType.House => _faker.Random.Number(1, 4),
            PropertyType.Townhouse => _faker.Random.Number(2, 4),
            PropertyType.Commercial => _faker.Random.Number(1, 10),
            PropertyType.Warehouse => _faker.Random.Number(1, 3),
            PropertyType.ParkingSpace => (int?)null,
            _ => _faker.Random.Number(1, 5)
        };

        var floor = totalFloors.HasValue ? _faker.Random.Number(1, totalFloors.Value) : (int?)null;

        var roomsCount = propertyType switch
        {
            PropertyType.Apartment => _faker.Random.Number(1, 5),
            PropertyType.House => _faker.Random.Number(3, 10),
            PropertyType.Townhouse => _faker.Random.Number(3, 8),
            _ => (int?)null
        };

        var totalArea = propertyType switch
        {
            PropertyType.Apartment => _faker.Random.Double(25, 150),
            PropertyType.House => _faker.Random.Double(80, 500),
            PropertyType.Townhouse => _faker.Random.Double(100, 300),
            PropertyType.Commercial => _faker.Random.Double(50, 1000),
            PropertyType.Warehouse => _faker.Random.Double(200, 5000),
            PropertyType.ParkingSpace => _faker.Random.Double(12, 30),
            _ => _faker.Random.Double(30, 100)
        };

        return new CreateRealEstatePropertyDto
        {
            Type = propertyType,
            Purpose = purpose,
            CadastralNumber = cadastralNumber,
            Address = $"г. {city}, ул. {street}, д. {houseNumber}{apartment}",
            TotalFloors = totalFloors,
            TotalArea = Math.Round(totalArea, 2),
            RoomsCount = roomsCount,
            CeilingHeight = _faker.Random.Bool(0.7f) ? Math.Round(_faker.Random.Double(2.5, 4.0), 2) : null,
            Floor = floor,
            HasEncumbrances = _faker.Random.Bool(0.1f)
        };
    }

    /// <summary>
    /// Генерирует DTO заявки
    /// </summary>
    public static CreateRequestDto GenerateRequest(Guid counterpartyId, Guid propertyId)
    {
        var requestType = _faker.PickRandom<RequestType>();

        var baseAmount = _faker.Random.Decimal(1_000_000, 50_000_000);

        return new CreateRequestDto
        {
            CounterpartyId = counterpartyId,
            PropertyId = propertyId,
            Type = requestType,
            Amount = Math.Round(baseAmount, 2),
            Date = _faker.Date.Between(DateTime.Now.AddYears(-2), DateTime.Now)
        };
    }

    /// <summary>
    /// Генерирует пакет данных: контрагент + недвижимость + заявка
    /// </summary>
    public static GeneratedDataPackage GenerateDataPackage()
    {
        return new GeneratedDataPackage
        {
            Counterparty = GenerateCounterparty(),
            Property = GenerateProperty()
        };
    }
}

/// <summary>
/// Пакет сгенерированных данных для отправки
/// </summary>
public class GeneratedDataPackage
{
    public required CreateCounterpartyDto Counterparty { get; init; }
    public required CreateRealEstatePropertyDto Property { get; init; }
}
