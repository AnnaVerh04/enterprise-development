using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.WebApi.Repositories;

/// <summary>
/// In-memory реализация репозитория контрагентов
/// </summary>
public class InMemoryCounterpartyRepository : ICounterpartyRepository
{
    private readonly List<Counterparty> _counterparties = [];
    private int _nextId = 1;

    public InMemoryCounterpartyRepository()
    {
        // Инициализация тестовыми данными
        SeedData();
    }

    private void SeedData()
    {
        var seedData = new[]
        {
            new Counterparty { Id = 1, FullName = "Иванов Иван Иванович", PassportNumber = "4501 123456", PhoneNumber = "+7-999-111-22-33" },
            new Counterparty { Id = 2, FullName = "Петрова Анна Сергеевна", PassportNumber = "4501 123457", PhoneNumber = "+7-999-111-22-34" },
            new Counterparty { Id = 3, FullName = "Сидоров Алексей Петрович", PassportNumber = "4501 123458", PhoneNumber = "+7-999-111-22-35" },
            new Counterparty { Id = 4, FullName = "Козлова Мария Владимировна", PassportNumber = "4501 123459", PhoneNumber = "+7-999-111-22-36" },
            new Counterparty { Id = 5, FullName = "Николаев Дмитрий Олегович", PassportNumber = "4501 123460", PhoneNumber = "+7-999-111-22-37" },
            new Counterparty { Id = 6, FullName = "Федоров Сергей Викторович", PassportNumber = "4501 123461", PhoneNumber = "+7-999-111-22-38" },
            new Counterparty { Id = 7, FullName = "Орлова Екатерина Дмитриевна", PassportNumber = "4501 123462", PhoneNumber = "+7-999-111-22-39" },
            new Counterparty { Id = 8, FullName = "Волков Павел Александрович", PassportNumber = "4501 123463", PhoneNumber = "+7-999-111-22-40" },
            new Counterparty { Id = 9, FullName = "Семенова Ольга Игоревна", PassportNumber = "4501 123464", PhoneNumber = "+7-999-111-22-41" },
            new Counterparty { Id = 10, FullName = "Морозов Андрей Сергеевич", PassportNumber = "4501 123465", PhoneNumber = "+7-999-111-22-42" },
            new Counterparty { Id = 11, FullName = "Зайцева Наталья Петровна", PassportNumber = "4501 123466", PhoneNumber = "+7-999-111-22-43" },
            new Counterparty { Id = 12, FullName = "Белов Игорь Васильевич", PassportNumber = "4501 123467", PhoneNumber = "+7-999-111-22-44" }
        };

        _counterparties.AddRange(seedData);
        _nextId = seedData.Length + 1;
    }

    public IEnumerable<Counterparty> GetAll() => _counterparties;

    public Counterparty? GetById(int id) => _counterparties.FirstOrDefault(c => c.Id == id);

    public Counterparty Add(Counterparty counterparty)
    {
        counterparty.Id = _nextId++;
        _counterparties.Add(counterparty);
        return counterparty;
    }

    public Counterparty? Update(int id, Counterparty counterparty)
    {
        var existing = GetById(id);
        if (existing == null) return null;

        existing.FullName = counterparty.FullName;
        existing.PassportNumber = counterparty.PassportNumber;
        existing.PhoneNumber = counterparty.PhoneNumber;
        return existing;
    }

    public bool Delete(int id)
    {
        var counterparty = GetById(id);
        if (counterparty == null) return false;

        _counterparties.Remove(counterparty);
        return true;
    }
}
