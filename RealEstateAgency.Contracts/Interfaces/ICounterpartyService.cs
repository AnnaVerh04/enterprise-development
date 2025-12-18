using RealEstateAgency.Contracts.Dto;

namespace RealEstateAgency.Contracts.Interfaces;

/// <summary>
/// Интерфейс сервиса контрагентов
/// </summary>
public interface ICounterpartyService : IApplicationService<CounterpartyDto, CreateCounterpartyDto, Guid>
{
    /// <summary>
    /// Обновить контрагента
    /// </summary>
    public Task<CounterpartyDto?> UpdateAsync(Guid id, UpdateCounterpartyDto dto);
}
