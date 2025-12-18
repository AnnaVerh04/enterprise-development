using AutoMapper;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Сервис объектов недвижимости
/// </summary>
public class RealEstatePropertyService(IRealEstatePropertyRepository repository, IMapper mapper) : IRealEstatePropertyService
{
    /// <inheritdoc />
    public async Task<IEnumerable<RealEstatePropertyDto>> GetAllAsync()
    {
        var properties = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<RealEstatePropertyDto>>(properties);
    }

    /// <inheritdoc />
    public async Task<RealEstatePropertyDto?> GetByIdAsync(Guid id)
    {
        var property = await repository.GetByIdAsync(id);
        return property == null ? null : mapper.Map<RealEstatePropertyDto>(property);
    }

    /// <inheritdoc />
    public async Task<RealEstatePropertyDto> CreateAsync(CreateRealEstatePropertyDto dto)
    {
        var property = mapper.Map<RealEstateProperty>(dto);
        var created = await repository.AddAsync(property);
        return mapper.Map<RealEstatePropertyDto>(created);
    }

    /// <inheritdoc />
    public async Task<RealEstatePropertyDto?> UpdateAsync(Guid id, CreateRealEstatePropertyDto dto)
    {
        var property = mapper.Map<RealEstateProperty>(dto);
        var updated = await repository.UpdateAsync(id, property);
        return updated == null ? null : mapper.Map<RealEstatePropertyDto>(updated);
    }

    /// <inheritdoc />
    public async Task<RealEstatePropertyDto?> UpdateAsync(Guid id, UpdateRealEstatePropertyDto dto)
    {
        var property = mapper.Map<RealEstateProperty>(dto);
        var updated = await repository.UpdateAsync(id, property);
        return updated == null ? null : mapper.Map<RealEstatePropertyDto>(updated);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await repository.DeleteAsync(id);
    }
}
