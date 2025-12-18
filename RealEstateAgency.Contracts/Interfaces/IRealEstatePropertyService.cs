using RealEstateAgency.Contracts.Dto;

namespace RealEstateAgency.Contracts.Interfaces;

/// <summary>
/// Интерфейс сервиса недвижимости
/// </summary>
public interface IRealEstatePropertyService : IApplicationService<RealEstatePropertyDto, CreateRealEstatePropertyDto, Guid>
{
    /// <summary>
    /// Обновить объект недвижимости
    /// </summary>
    public Task<RealEstatePropertyDto?> UpdateAsync(Guid id, UpdateRealEstatePropertyDto dto);
}
