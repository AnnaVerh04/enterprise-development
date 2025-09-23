using RealEstateAgency.Domain.Models;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

public class RealEstateTestFixture
{
    public List<Counterparty> Counterparties { get; }
    public List<RealEstateProperty> Properties { get; }
    public List<Request> Requests { get; }

    public RealEstateTestFixture()
    {
        Counterparties = GenerateCounterparties();
    }

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
}