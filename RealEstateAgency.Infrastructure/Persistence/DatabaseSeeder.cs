using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Infrastructure.Persistence;

/// <summary>
/// Сервис для начального заполнения базы данных
/// </summary>
public class DatabaseSeeder(RealEstateDbContext context, ILogger<DatabaseSeeder> logger)
{

    /// <summary>
    /// Заполняет базу данных начальными данными, если она пуста
    /// </summary>
    public async Task SeedAsync()
    {
        var counterpartiesCount = await context.Counterparties.CountAsync();
        if (counterpartiesCount > 0)
        {
            logger.LogInformation("База данных уже содержит данные, seed пропущен");
            return;
        }

        logger.LogInformation("Начало заполнения базы данных...");

        var counterparties = GenerateCounterparties();
        await context.Counterparties.AddRangeAsync(counterparties);
        await context.SaveChangesAsync();
        logger.LogInformation("Добавлено {Count} контрагентов", counterparties.Count);

        var properties = GenerateProperties();
        await context.Properties.AddRangeAsync(properties);
        await context.SaveChangesAsync();
        logger.LogInformation("Добавлено {Count} объектов недвижимости", properties.Count);

        var requests = GenerateRequests(counterparties, properties);
        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();
        logger.LogInformation("Добавлено {Count} заявок", requests.Count);

        logger.LogInformation("Заполнение базы данных завершено");
    }

    private static List<Counterparty> GenerateCounterparties() =>
    [
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), FullName = "Иванов Иван Иванович", PassportNumber = "4501 123456", PhoneNumber = "+7-999-111-22-33" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), FullName = "Петрова Анна Сергеевна", PassportNumber = "4501 123457", PhoneNumber = "+7-999-111-22-34" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), FullName = "Сидоров Алексей Петрович", PassportNumber = "4501 123458", PhoneNumber = "+7-999-111-22-35" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), FullName = "Козлова Мария Владимировна", PassportNumber = "4501 123459", PhoneNumber = "+7-999-111-22-36" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), FullName = "Николаев Дмитрий Олегович", PassportNumber = "4501 123460", PhoneNumber = "+7-999-111-22-37" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), FullName = "Федоров Сергей Викторович", PassportNumber = "4501 123461", PhoneNumber = "+7-999-111-22-38" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), FullName = "Орлова Екатерина Дмитриевна", PassportNumber = "4501 123462", PhoneNumber = "+7-999-111-22-39" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), FullName = "Волков Павел Александрович", PassportNumber = "4501 123463", PhoneNumber = "+7-999-111-22-40" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), FullName = "Семенова Ольга Игоревна", PassportNumber = "4501 123464", PhoneNumber = "+7-999-111-22-41" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-00000000000a"), FullName = "Морозов Андрей Сергеевич", PassportNumber = "4501 123465", PhoneNumber = "+7-999-111-22-42" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-00000000000b"), FullName = "Зайцева Наталья Петровна", PassportNumber = "4501 123466", PhoneNumber = "+7-999-111-22-43" },
        new() { Id = Guid.Parse("00000000-0000-0000-0000-00000000000c"), FullName = "Белов Игорь Васильевич", PassportNumber = "4501 123467", PhoneNumber = "+7-999-111-22-44" }
    ];

    private static List<RealEstateProperty> GenerateProperties() =>
    [
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Type = PropertyType.Apartment, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:01:0001001:101", Address = "ул. Тверская, 15, кв. 34", TotalFloors = 9, TotalArea = 75.5, RoomsCount = 3, CeilingHeight = 2.7, Floor = 5, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Type = PropertyType.Apartment, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:01:0001002:102", Address = "ул. Арбат, 25, кв. 12", TotalFloors = 5, TotalArea = 45.0, RoomsCount = 2, CeilingHeight = 2.5, Floor = 3, HasEncumbrances = true },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Type = PropertyType.Apartment, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:01:0001003:103", Address = "пр-т Мира, 10, кв. 78", TotalFloors = 12, TotalArea = 90.0, RoomsCount = 4, CeilingHeight = 2.8, Floor = 8, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Type = PropertyType.House, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:02:0002001:201", Address = "Московская обл., коттеджный поселок 'Лесной', д. 12", TotalFloors = 2, TotalArea = 150.0, RoomsCount = 6, CeilingHeight = 3.0, Floor = null, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Type = PropertyType.House, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:02:0002002:202", Address = "Московская обл., д. Пушкино, ул. Садовая, 5", TotalFloors = 1, TotalArea = 80.0, RoomsCount = 4, CeilingHeight = 2.6, Floor = null, HasEncumbrances = true },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Type = PropertyType.Townhouse, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:03:0003001:301", Address = "пос. Рублево, таунхаусный комплекс 'Резиденция', к. 7", TotalFloors = 3, TotalArea = 120.0, RoomsCount = 5, CeilingHeight = 2.7, Floor = null, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Type = PropertyType.Townhouse, Purpose = PropertyPurpose.Residential, CadastralNumber = "77:03:0003002:302", Address = "пос. Барвиха, таунхаусный комплекс 'Престиж', к. 3", TotalFloors = 2, TotalArea = 95.0, RoomsCount = 4, CeilingHeight = 2.8, Floor = null, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Type = PropertyType.Commercial, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:05:0005001:501", Address = "ул. Новый Арбат, 15, офис 300", TotalFloors = 10, TotalArea = 60.0, RoomsCount = 2, CeilingHeight = 2.8, Floor = 3, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Type = PropertyType.Commercial, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:05:0005002:502", Address = "ул. Тверская-Ямская, 8, магазин", TotalFloors = 3, TotalArea = 85.0, RoomsCount = 1, CeilingHeight = 3.2, Floor = 1, HasEncumbrances = true },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-00000000000a"), Type = PropertyType.ParkingSpace, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:06:0006001:601", Address = "ул. Садовая-Кудринская, 1, подземный паркинг, место А-15", TotalFloors = null, TotalArea = 12.5, RoomsCount = null, CeilingHeight = 2.2, Floor = -1, HasEncumbrances = true },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-00000000000b"), Type = PropertyType.ParkingSpace, Purpose = PropertyPurpose.Commercial, CadastralNumber = "77:06:0006002:602", Address = "ул. Мясницкая, 20, паркинг, место Б-07", TotalFloors = null, TotalArea = 13.0, RoomsCount = null, CeilingHeight = 2.3, Floor = -2, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-00000000000c"), Type = PropertyType.Warehouse, Purpose = PropertyPurpose.Industrial, CadastralNumber = "77:07:0007001:701", Address = "промзона 'Южные Ворота', складской комплекс №3", TotalFloors = 1, TotalArea = 500.0, RoomsCount = null, CeilingHeight = 6.0, Floor = null, HasEncumbrances = false },
        new() { Id = Guid.Parse("10000000-0000-0000-0000-00000000000d"), Type = PropertyType.Warehouse, Purpose = PropertyPurpose.Industrial, CadastralNumber = "77:07:0007002:702", Address = "промзона 'Северная', склад №5", TotalFloors = 2, TotalArea = 350.0, RoomsCount = null, CeilingHeight = 5.5, Floor = null, HasEncumbrances = true }
    ];

    private static List<Request> GenerateRequests(List<Counterparty> counterparties, List<RealEstateProperty> properties) =>
    [
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Counterparty = counterparties[0], Property = properties[0], Type = RequestType.Sale, Amount = 25000000.00m, Date = new DateTime(2024, 1, 15) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Counterparty = counterparties[1], Property = properties[1], Type = RequestType.Sale, Amount = 18000000.00m, Date = new DateTime(2024, 2, 20) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Counterparty = counterparties[3], Property = properties[3], Type = RequestType.Sale, Amount = 42000000.00m, Date = new DateTime(2024, 3, 10) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Counterparty = counterparties[6], Property = properties[5], Type = RequestType.Sale, Amount = 35000000.00m, Date = new DateTime(2024, 4, 5) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000005"), Counterparty = counterparties[8], Property = properties[7], Type = RequestType.Sale, Amount = 32000000.00m, Date = new DateTime(2024, 5, 12) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000006"), Counterparty = counterparties[10], Property = properties[9], Type = RequestType.Sale, Amount = 1500000.00m, Date = new DateTime(2024, 6, 8) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000007"), Counterparty = counterparties[11], Property = properties[11], Type = RequestType.Sale, Amount = 85000000.00m, Date = new DateTime(2024, 7, 25) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000008"), Counterparty = counterparties[2], Property = properties[2], Type = RequestType.Purchase, Amount = 22000000.00m, Date = new DateTime(2024, 1, 20) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-000000000009"), Counterparty = counterparties[4], Property = properties[4], Type = RequestType.Purchase, Amount = 15000000.00m, Date = new DateTime(2024, 2, 25) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-00000000000a"), Counterparty = counterparties[5], Property = properties[6], Type = RequestType.Purchase, Amount = 28000000.00m, Date = new DateTime(2024, 3, 15) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-00000000000b"), Counterparty = counterparties[7], Property = properties[8], Type = RequestType.Purchase, Amount = 25000000.00m, Date = new DateTime(2024, 4, 18) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-00000000000c"), Counterparty = counterparties[9], Property = properties[10], Type = RequestType.Purchase, Amount = 1800000.00m, Date = new DateTime(2024, 5, 22) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-00000000000d"), Counterparty = counterparties[2], Property = properties[12], Type = RequestType.Purchase, Amount = 60000000.00m, Date = new DateTime(2024, 6, 30) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-00000000000e"), Counterparty = counterparties[1], Property = properties[0], Type = RequestType.Purchase, Amount = 24000000.00m, Date = new DateTime(2024, 8, 10) },
        new() { Id = Guid.Parse("20000000-0000-0000-0000-00000000000f"), Counterparty = counterparties[3], Property = properties[1], Type = RequestType.Sale, Amount = 19000000.00m, Date = new DateTime(2024, 9, 5) }
    ];
}
