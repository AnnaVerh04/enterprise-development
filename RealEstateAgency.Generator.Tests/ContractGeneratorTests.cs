using FluentAssertions;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Generator.Generators;

namespace RealEstateAgency.Generator.Tests;

/// <summary>
/// Тесты генератора контрактов
/// </summary>
public class ContractGeneratorTests
{

    [Fact]
    public void GenerateCounterparty_ShouldReturnValidCounterparty()
    {
        var counterparty = ContractGenerator.GenerateCounterparty();

        counterparty.Should().NotBeNull();
        counterparty.FullName.Should().NotBeNullOrEmpty();
        counterparty.PassportNumber.Should().NotBeNullOrEmpty();
        counterparty.PhoneNumber.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateCounterparty_FullName_ShouldContainThreeParts()
    {
        var counterparty = ContractGenerator.GenerateCounterparty();

        var nameParts = counterparty.FullName.Split(' ');
        nameParts.Should().HaveCount(3, "ФИО должно состоять из фамилии, имени и отчества");
    }

    [Fact]
    public void GenerateCounterparty_PassportNumber_ShouldHaveValidFormat()
    {
        var counterparty = ContractGenerator.GenerateCounterparty();

        counterparty.PassportNumber.Should().MatchRegex(@"^\d{4}\s\d{6}$",
            "Номер паспорта должен быть в формате 'XXXX XXXXXX'");
    }

    [Fact]
    public void GenerateCounterparty_PhoneNumber_ShouldStartWithPlus7()
    {
        var counterparty = ContractGenerator.GenerateCounterparty();

        counterparty.PhoneNumber.Should().StartWith("+7",
            "Российский номер должен начинаться с +7");

        var cleanNumber = counterparty.PhoneNumber.Replace("+7", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        cleanNumber.Should().MatchRegex(@"^[0-9]{10}$",
            "Номер должен содержать 10 цифр после +7");
    }

    [Fact]
    public void GenerateCounterparty_ShouldGenerateUniqueData()
    {
        var counterparties = Enumerable.Range(0, 100)
            .Select(_ => ContractGenerator.GenerateCounterparty())
            .ToList();

        var uniquePassports = counterparties.Select(c => c.PassportNumber).Distinct().Count();
        uniquePassports.Should().BeGreaterThan(90,
            "Большинство номеров паспортов должны быть уникальными");
    }


    [Fact]
    public void GenerateProperty_ShouldReturnValidProperty()
    {
        var property = ContractGenerator.GenerateProperty();

        property.Should().NotBeNull();
        property.CadastralNumber.Should().NotBeNullOrEmpty();
        property.Address.Should().NotBeNullOrEmpty();
        property.TotalArea.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GenerateProperty_CadastralNumber_ShouldHaveValidFormat()
    {
        var property = ContractGenerator.GenerateProperty();

        property.CadastralNumber.Should().MatchRegex(@"^\d{2}:\d{2}:\d{7}:\d{3}$",
            "Кадастровый номер должен быть в формате XX:XX:XXXXXXX:XXX");
    }

    [Fact]
    public void GenerateProperty_Address_ShouldContainCity()
    {
        var property = ContractGenerator.GenerateProperty();

        property.Address.Should().StartWith("г.",
            "Адрес должен начинаться с 'г.' (город)");
    }

    [Fact]
    public void GenerateProperty_TotalArea_ShouldBeInValidRange()
    {
        var properties = Enumerable.Range(0, 100)
            .Select(_ => ContractGenerator.GenerateProperty())
            .ToList();

        foreach (var property in properties)
        {
            property.TotalArea.Should().BeGreaterThan(0);
            property.TotalArea.Should().BeLessThanOrEqualTo(5000,
                "Площадь не должна превышать 5000 кв.м");
        }
    }

    [Fact]
    public void GenerateProperty_ShouldGenerateAllPropertyTypes()
    {
        var properties = Enumerable.Range(0, 500)
            .Select(_ => ContractGenerator.GenerateProperty())
            .ToList();

        var propertyTypes = properties.Select(p => p.Type).Distinct().ToList();
        propertyTypes.Should().Contain(PropertyType.Apartment);
        propertyTypes.Should().Contain(PropertyType.House);
    }

    [Fact]
    public void GenerateProperty_Apartment_ShouldHaveValidFloorAndTotalFloors()
    {
        for (var i = 0; i < 100; i++)
        {
            var property = ContractGenerator.GenerateProperty();

            if (property.Type == PropertyType.Apartment &&
                property.Floor.HasValue &&
                property.TotalFloors.HasValue)
            {
                property.Floor.Value.Should().BeLessThanOrEqualTo(property.TotalFloors.Value,
                    "Этаж не должен превышать количество этажей в доме");
                property.Floor.Value.Should().BeGreaterThanOrEqualTo(1);
            }
        }
    }

    [Fact]
    public void GenerateProperty_ResidentialTypes_ShouldHaveResidentialPurpose()
    {
        var properties = Enumerable.Range(0, 200)
            .Select(_ => ContractGenerator.GenerateProperty())
            .Where(p => p.Type == PropertyType.Apartment ||
                       p.Type == PropertyType.House ||
                       p.Type == PropertyType.Townhouse)
            .ToList();

        foreach (var property in properties)
        {
            property.Purpose.Should().Be(PropertyPurpose.Residential,
                $"Тип {property.Type} должен иметь жилое назначение");
        }
    }

    [Fact]
    public void GenerateProperty_Commercial_ShouldHaveCommercialPurpose()
    {
        var properties = Enumerable.Range(0, 200)
            .Select(_ => ContractGenerator.GenerateProperty())
            .Where(p => p.Type == PropertyType.Commercial)
            .ToList();

        foreach (var property in properties)
        {
            property.Purpose.Should().Be(PropertyPurpose.Commercial,
                "Коммерческая недвижимость должна иметь коммерческое назначение");
        }
    }

    [Fact]
    public void GenerateProperty_Warehouse_ShouldHaveIndustrialPurpose()
    {
        var properties = Enumerable.Range(0, 200)
            .Select(_ => ContractGenerator.GenerateProperty())
            .Where(p => p.Type == PropertyType.Warehouse)
            .ToList();

        foreach (var property in properties)
        {
            property.Purpose.Should().Be(PropertyPurpose.Industrial,
                "Склад должен иметь промышленное назначение");
        }
    }

    [Fact]
    public void GenerateProperty_CeilingHeight_ShouldBeInValidRange()
    {
        var properties = Enumerable.Range(0, 100)
            .Select(_ => ContractGenerator.GenerateProperty())
            .Where(p => p.CeilingHeight.HasValue)
            .ToList();

        foreach (var property in properties)
        {
            property.CeilingHeight!.Value.Should().BeGreaterThanOrEqualTo(2.5);
            property.CeilingHeight!.Value.Should().BeLessThanOrEqualTo(4.0);
        }
    }


    [Fact]
    public void GenerateRequest_ShouldReturnValidRequest()
    {
        var counterpartyId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();

        var request = ContractGenerator.GenerateRequest(counterpartyId, propertyId);

        request.Should().NotBeNull();
        request.CounterpartyId.Should().Be(counterpartyId);
        request.PropertyId.Should().Be(propertyId);
        request.Amount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GenerateRequest_Amount_ShouldBeInValidRange()
    {
        var counterpartyId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();

        var requests = Enumerable.Range(0, 100)
            .Select(_ => ContractGenerator.GenerateRequest(counterpartyId, propertyId))
            .ToList();

        foreach (var request in requests)
        {
            request.Amount.Should().BeGreaterThanOrEqualTo(1_000_000m,
                "Минимальная сумма сделки должна быть 1 000 000");
            request.Amount.Should().BeLessThanOrEqualTo(50_000_000m,
                "Максимальная сумма сделки должна быть 50 000 000");
        }
    }

    [Fact]
    public void GenerateRequest_Date_ShouldBeWithinLastTwoYears()
    {
        var counterpartyId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var twoYearsAgo = DateTime.Now.AddYears(-2);

        var requests = Enumerable.Range(0, 100)
            .Select(_ => ContractGenerator.GenerateRequest(counterpartyId, propertyId))
            .ToList();

        foreach (var request in requests)
        {
            request.Date.Should().BeAfter(twoYearsAgo);
            request.Date.Should().BeOnOrBefore(DateTime.Now);
        }
    }

    [Fact]
    public void GenerateRequest_ShouldGenerateBothRequestTypes()
    {
        var counterpartyId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();

        var requests = Enumerable.Range(0, 100)
            .Select(_ => ContractGenerator.GenerateRequest(counterpartyId, propertyId))
            .ToList();

        var requestTypes = requests.Select(r => r.Type).Distinct().ToList();
        requestTypes.Should().Contain(RequestType.Purchase);
        requestTypes.Should().Contain(RequestType.Sale);
    }


    [Fact]
    public void GenerateDataPackage_ShouldReturnValidPackage()
    {
        var package = ContractGenerator.GenerateDataPackage();

        package.Should().NotBeNull();
        package.Counterparty.Should().NotBeNull();
        package.Property.Should().NotBeNull();
    }

    [Fact]
    public void GenerateDataPackage_ShouldGenerateUniquePackages()
    {
        var packages = Enumerable.Range(0, 50)
            .Select(_ => ContractGenerator.GenerateDataPackage())
            .ToList();

        var uniquePassports = packages
            .Select(p => p.Counterparty.PassportNumber)
            .Distinct()
            .Count();

        uniquePassports.Should().BeGreaterThan(40,
            "Большинство пакетов должны содержать уникальные данные");
    }

}
