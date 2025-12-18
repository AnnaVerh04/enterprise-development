using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Infrastructure.Repositories;

/// <summary>
/// In-memory реализация репозитория контрагентов
/// </summary>
public class InMemoryCounterpartyRepository : ICounterpartyRepository
{
    private readonly List<Counterparty> _counterparties = [];

    public InMemoryCounterpartyRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        var seedData = new[]
        {
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), FullName = "Иванов Иван Иванович", PassportNumber = "4501 123456", PhoneNumber = "+7-999-111-22-33" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), FullName = "Петрова Анна Сергеевна", PassportNumber = "4501 123457", PhoneNumber = "+7-999-111-22-34" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), FullName = "Сидоров Алексей Петрович", PassportNumber = "4501 123458", PhoneNumber = "+7-999-111-22-35" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), FullName = "Козлова Мария Владимировна", PassportNumber = "4501 123459", PhoneNumber = "+7-999-111-22-36" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), FullName = "Николаев Дмитрий Олегович", PassportNumber = "4501 123460", PhoneNumber = "+7-999-111-22-37" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), FullName = "Федоров Сергей Викторович", PassportNumber = "4501 123461", PhoneNumber = "+7-999-111-22-38" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), FullName = "Орлова Екатерина Дмитриевна", PassportNumber = "4501 123462", PhoneNumber = "+7-999-111-22-39" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), FullName = "Волков Павел Александрович", PassportNumber = "4501 123463", PhoneNumber = "+7-999-111-22-40" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), FullName = "Семенова Ольга Игоревна", PassportNumber = "4501 123464", PhoneNumber = "+7-999-111-22-41" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-00000000000a"), FullName = "Морозов Андрей Сергеевич", PassportNumber = "4501 123465", PhoneNumber = "+7-999-111-22-42" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-00000000000b"), FullName = "Зайцева Наталья Петровна", PassportNumber = "4501 123466", PhoneNumber = "+7-999-111-22-43" },
            new Counterparty { Id = Guid.Parse("00000000-0000-0000-0000-00000000000c"), FullName = "Белов Игорь Васильевич", PassportNumber = "4501 123467", PhoneNumber = "+7-999-111-22-44" }
        };

        _counterparties.AddRange(seedData);
    }

    public Task<IEnumerable<Counterparty>> GetAllAsync() => Task.FromResult<IEnumerable<Counterparty>>(_counterparties);

    public Task<Counterparty?> GetByIdAsync(Guid id) => Task.FromResult(_counterparties.FirstOrDefault(c => c.Id == id));

    public Task<Counterparty> AddAsync(Counterparty counterparty)
    {
        counterparty.Id = Guid.NewGuid();
        _counterparties.Add(counterparty);
        return Task.FromResult(counterparty);
    }

    public Task<Counterparty?> UpdateAsync(Guid id, Counterparty counterparty)
    {
        var existing = _counterparties.FirstOrDefault(c => c.Id == id);
        if (existing == null) return Task.FromResult<Counterparty?>(null);

        existing.FullName = counterparty.FullName;
        existing.PassportNumber = counterparty.PassportNumber;
        existing.PhoneNumber = counterparty.PhoneNumber;
        return Task.FromResult<Counterparty?>(existing);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var counterparty = _counterparties.FirstOrDefault(c => c.Id == id);
        if (counterparty == null) return Task.FromResult(false);

        _counterparties.Remove(counterparty);
        return Task.FromResult(true);
    }
}
