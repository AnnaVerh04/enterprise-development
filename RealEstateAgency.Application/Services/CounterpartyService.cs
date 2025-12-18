using AutoMapper;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Сервис контрагентов
/// </summary>
public class CounterpartyService(ICounterpartyRepository repository, IMapper mapper) : ICounterpartyService
{
    /// <inheritdoc />
    public async Task<IEnumerable<CounterpartyDto>> GetAllAsync()
    {
        var counterparties = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<CounterpartyDto>>(counterparties);
    }

    /// <inheritdoc />
    public async Task<CounterpartyDto?> GetByIdAsync(Guid id)
    {
        var counterparty = await repository.GetByIdAsync(id);
        return counterparty == null ? null : mapper.Map<CounterpartyDto>(counterparty);
    }

    /// <inheritdoc />
    public async Task<CounterpartyDto> CreateAsync(CreateCounterpartyDto dto)
    {
        var counterparty = mapper.Map<Counterparty>(dto);
        var created = await repository.AddAsync(counterparty);
        return mapper.Map<CounterpartyDto>(created);
    }

    /// <inheritdoc />
    public async Task<CounterpartyDto?> UpdateAsync(Guid id, CreateCounterpartyDto dto)
    {
        var counterparty = mapper.Map<Counterparty>(dto);
        var updated = await repository.UpdateAsync(id, counterparty);
        return updated == null ? null : mapper.Map<CounterpartyDto>(updated);
    }

    /// <inheritdoc />
    public async Task<CounterpartyDto?> UpdateAsync(Guid id, UpdateCounterpartyDto dto)
    {
        var counterparty = mapper.Map<Counterparty>(dto);
        var updated = await repository.UpdateAsync(id, counterparty);
        return updated == null ? null : mapper.Map<CounterpartyDto>(updated);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await repository.DeleteAsync(id);
    }
}
