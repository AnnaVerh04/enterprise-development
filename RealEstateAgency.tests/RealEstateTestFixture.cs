using RealEstateAgency.Domain.Models;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

/// <summary>
/// The fixture for the real estate agency's test data
/// It contains collections of counterparties, real estate objects, and applications
/// </summary>
public class RealEstateTestFixture
{
    public List<Counterparty> Counterparties { get; }
    public List<RealEstateProperty> Properties { get; }
    public List<Request> Requests { get; }

    /// <summary>
    /// Initializes the test data
    /// </summary>
    public RealEstateTestFixture()
    {
        Counterparties = GenerateCounterparties();
        Properties = GenerateProperties();
        Requests = GenerateRequests(Counterparties, Properties);
    }

    /// <summary>
    /// Generates test counterparties
    /// </summary>
    private List<Counterparty> GenerateCounterparties()
    {
        return new List<Counterparty>
        {
            new() { Id = 1, FullName = "Иванов Иван Иванович", PassportNumber = "4501 123456", PhoneNumber = "+7-999-111-22-33" },
            new() { Id = 2, FullName = "Петрова Анна Сергеевна", PassportNumber = "4501 123457", PhoneNumber = "+7-999-111-22-34" },
            new() { Id = 3, FullName = "Сидоров Алексей Петрович", PassportNumber = "4501 123458", PhoneNumber = "+7-999-111-22-35" },
            new() { Id = 4, FullName = "Козлова Мария Владимировна", PassportNumber = "4501 123459", PhoneNumber = "+7-999-111-22-36" },
            new() { Id = 5, FullName = "Николаев Дмитрий Олегович", PassportNumber = "4501 123460", PhoneNumber = "+7-999-111-22-37" },
            new() { Id = 6, FullName = "Федоров Сергей Викторович", PassportNumber = "4501 123461", PhoneNumber = "+7-999-111-22-38" },
            new() { Id = 7, FullName = "Орлова Екатерина Дмитриевна", PassportNumber = "4501 123462", PhoneNumber = "+7-999-111-22-39" },
            new() { Id = 8, FullName = "Волков Павел Александрович", PassportNumber = "4501 123463", PhoneNumber = "+7-999-111-22-40" },
            new() { Id = 9, FullName = "Семенова Ольга Игоревна", PassportNumber = "4501 123464", PhoneNumber = "+7-999-111-22-41" },
            new() { Id = 10, FullName = "Морозов Андрей Сергеевич", PassportNumber = "4501 123465", PhoneNumber = "+7-999-111-22-42" },
            new() { Id = 11, FullName = "Зайцева Наталья Петровна", PassportNumber = "4501 123466", PhoneNumber = "+7-999-111-22-43" },
            new() { Id = 12, FullName = "Белов Игорь Васильевич", PassportNumber = "4501 123467", PhoneNumber = "+7-999-111-22-44" }
        };
    }

    /// <summary>
    /// Generates test properties
    /// </summary>
    private List<RealEstateProperty> GenerateProperties()
    {
        return new List<RealEstateProperty>
    {
        new() {
            Id = 1,
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:01:0001001:101",
            Address = "ул. Тверская, 15, кв. 34",
            TotalFloors = 9,
            TotalArea = 75.5,
            RoomsCount = 3,
            CeilingHeight = 2.7,
            Floor = 5,
            HasEncumbrances = false
        },
        new() {
            Id = 2,
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:01:0001002:102",
            Address = "ул. Арбат, 25, кв. 12",
            TotalFloors = 5,
            TotalArea = 45.0,
            RoomsCount = 2,
            CeilingHeight = 2.5,
            Floor = 3,
            HasEncumbrances = true
        },
        new() {
            Id = 3,
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:01:0001003:103",
            Address = "пр-т Мира, 10, кв. 78",
            TotalFloors = 12,
            TotalArea = 90.0,
            RoomsCount = 4,
            CeilingHeight = 2.8,
            Floor = 8,
            HasEncumbrances = false
        },

        new() {
            Id = 4,
            Type = PropertyType.House,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:02:0002001:201",
            Address = "Московская обл., коттеджный поселок 'Лесной', д. 12",
            TotalFloors = 2,
            TotalArea = 150.0,
            RoomsCount = 6,
            CeilingHeight = 3.0,
            Floor = null,
            HasEncumbrances = false
        },
        new() {
            Id = 5,
            Type = PropertyType.House,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:02:0002002:202",
            Address = "Московская обл., д. Пушкино, ул. Садовая, 5",
            TotalFloors = 1,
            TotalArea = 80.0,
            RoomsCount = 4,
            CeilingHeight = 2.6,
            Floor = null,
            HasEncumbrances = true
        },

        new() {
            Id = 6,
            Type = PropertyType.Townhouse,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:03:0003001:301",
            Address = "пос. Рублево, таунхаусный комплекс 'Резиденция', к. 7",
            TotalFloors = 3,
            TotalArea = 120.0,
            RoomsCount = 5,
            CeilingHeight = 2.7,
            Floor = null,
            HasEncumbrances = false
        },
        new() {
            Id = 7,
            Type = PropertyType.Townhouse,
            Purpose = PropertyPurpose.Residential,
            CadastralNumber = "77:03:0003002:302",
            Address = "пос. Барвиха, таунхаусный комплекс 'Престиж', к. 3",
            TotalFloors = 2,
            TotalArea = 95.0,
            RoomsCount = 4,
            CeilingHeight = 2.8,
            Floor = null,
            HasEncumbrances = false
        },

        new() {
            Id = 8,
            Type = PropertyType.Commercial,
            Purpose = PropertyPurpose.Commercial,
            CadastralNumber = "77:05:0005001:501",
            Address = "ул. Новый Арбат, 15, офис 300",
            TotalFloors = 10,
            TotalArea = 60.0,
            RoomsCount = 2,
            CeilingHeight = 2.8,
            Floor = 3,
            HasEncumbrances = false
        },
        new() {
            Id = 9,
            Type = PropertyType.Commercial,
            Purpose = PropertyPurpose.Commercial,
            CadastralNumber = "77:05:0005002:502",
            Address = "ул. Тверская-Ямская, 8, магазин",
            TotalFloors = 3,
            TotalArea = 85.0,
            RoomsCount = 1,
            CeilingHeight = 3.2,
            Floor = 1,
            HasEncumbrances = true
        },

        new() {
            Id = 10,
            Type = PropertyType.ParkingSpace,
            Purpose = PropertyPurpose.Commercial,
            CadastralNumber = "77:06:0006001:601",
            Address = "ул. Садовая-Кудринская, 1, подземный паркинг, место А-15",
            TotalFloors = null,
            TotalArea = 12.5,
            RoomsCount = null,
            CeilingHeight = 2.2,
            Floor = -1,
            HasEncumbrances = true
        },
        new() {
            Id = 11,
            Type = PropertyType.ParkingSpace,
            Purpose = PropertyPurpose.Commercial,
            CadastralNumber = "77:06:0006002:602",
            Address = "ул. Мясницкая, 20, паркинг, место Б-07",
            TotalFloors = null,
            TotalArea = 13.0,
            RoomsCount = null,
            CeilingHeight = 2.3,
            Floor = -2,
            HasEncumbrances = false
        },

        new() {
            Id = 12,
            Type = PropertyType.Warehouse,
            Purpose = PropertyPurpose.Industrial,
            CadastralNumber = "77:07:0007001:701",
            Address = "промзона 'Южные Ворота', складской комплекс №3",
            TotalFloors = 1,
            TotalArea = 500.0,
            RoomsCount = null,
            CeilingHeight = 6.0,
            Floor = null,
            HasEncumbrances = false
        },
        new() {
            Id = 13,
            Type = PropertyType.Warehouse,
            Purpose = PropertyPurpose.Industrial,
            CadastralNumber = "77:07:0007002:702",
            Address = "промзона 'Северная', склад №5",
            TotalFloors = 2,
            TotalArea = 350.0,
            RoomsCount = null,
            CeilingHeight = 5.5,
            Floor = null,
            HasEncumbrances = true
        }
    };
    }

    /// <summary>
    /// Generates test applications and connects them with contractors and facilities
    /// </summary>
    private List<Request> GenerateRequests(List<Counterparty> counterparties, List<RealEstateProperty> properties)
    {
        return new List<Request>
    {
        new() {
            Id = 1,
            Counterparty = counterparties[0], 
            Property = properties[0],         
            Type = RequestType.Sale,
            Amount = 25000000.00m,
            Date = new DateTime(2024, 1, 15)
        },
        new() {
            Id = 2,
            Counterparty = counterparties[1], 
            Property = properties[1],         
            Type = RequestType.Sale,
            Amount = 18000000.00m,
            Date = new DateTime(2024, 2, 20)
        },
        new() {
            Id = 3,
            Counterparty = counterparties[3], 
            Property = properties[3],         
            Type = RequestType.Sale,
            Amount = 42000000.00m,
            Date = new DateTime(2024, 3, 10)
        },
        new() {
            Id = 4,
            Counterparty = counterparties[6], 
            Property = properties[5],         
            Type = RequestType.Sale,
            Amount = 35000000.00m,
            Date = new DateTime(2024, 4, 5)
        },
        new() {
            Id = 5,
            Counterparty = counterparties[8], 
            Property = properties[7],         
            Type = RequestType.Sale,
            Amount = 32000000.00m,
            Date = new DateTime(2024, 5, 12)
        },
        new() {
            Id = 6,
            Counterparty = counterparties[10], 
            Property = properties[9],          
            Type = RequestType.Sale,
            Amount = 1500000.00m,
            Date = new DateTime(2024, 6, 8)
        },
        new() {
            Id = 7,
            Counterparty = counterparties[11], 
            Property = properties[11],         
            Type = RequestType.Sale,
            Amount = 85000000.00m,
            Date = new DateTime(2024, 7, 25)
        },

        new() {
            Id = 8,
            Counterparty = counterparties[2],  
            Property = properties[2],          
            Type = RequestType.Purchase,
            Amount = 22000000.00m,
            Date = new DateTime(2024, 1, 20)
        },
        new() {
            Id = 9,
            Counterparty = counterparties[4],  
            Property = properties[4],          
            Type = RequestType.Purchase,
            Amount = 15000000.00m,
            Date = new DateTime(2024, 2, 25)
        },
        new() {
            Id = 10,
            Counterparty = counterparties[5],  
            Property = properties[6],          
            Type = RequestType.Purchase,
            Amount = 28000000.00m,
            Date = new DateTime(2024, 3, 15)
        },
        new() {
            Id = 11,
            Counterparty = counterparties[7],  
            Property = properties[8],          
            Type = RequestType.Purchase,
            Amount = 25000000.00m,
            Date = new DateTime(2024, 4, 18)
        },
        new() {
            Id = 12,
            Counterparty = counterparties[9],  
            Property = properties[10],         
            Type = RequestType.Purchase,
            Amount = 1800000.00m,
            Date = new DateTime(2024, 5, 22)
        },
        new() {
            Id = 13,
            Counterparty = counterparties[2],  
            Property = properties[12],         
            Type = RequestType.Purchase,
            Amount = 60000000.00m,
            Date = new DateTime(2024, 6, 30)
        },

        new() {
            Id = 14,
            Counterparty = counterparties[1],  
            Property = properties[0],          
            Type = RequestType.Purchase,
            Amount = 24000000.00m,
            Date = new DateTime(2024, 8, 10)
        },
        new() {
            Id = 15,
            Counterparty = counterparties[3],  
            Property = properties[1],          
            Type = RequestType.Sale,
            Amount = 19000000.00m,
            Date = new DateTime(2024, 9, 5)
        }
    };
    }
}