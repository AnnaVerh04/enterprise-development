using AutoMapper;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;
using RealEstateAgency.Domain.Interfaces;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Сервис заявок
/// </summary>
public class RequestService(
    IRequestRepository requestRepository,
    ICounterpartyRepository counterpartyRepository,
    IRealEstatePropertyRepository propertyRepository,
    IMapper mapper) : IRequestService
{
    /// <inheritdoc />
    public async Task<IEnumerable<RequestDto>> GetAllAsync()
    {
        var requests = await requestRepository.GetAllAsync();
        return mapper.Map<IEnumerable<RequestDto>>(requests);
    }

    /// <inheritdoc />
    public async Task<RequestDto?> GetByIdAsync(Guid id)
    {
        var request = await requestRepository.GetByIdAsync(id);
        return request == null ? null : mapper.Map<RequestDto>(request);
    }

    /// <inheritdoc />
    public async Task<(RequestDto? Result, string? Error)> CreateAsync(CreateRequestDto dto)
    {
        var counterparty = await counterpartyRepository.GetByIdAsync(dto.CounterpartyId);
        if (counterparty == null)
            return (null, $"Контрагент с ID {dto.CounterpartyId} не найден");

        var property = await propertyRepository.GetByIdAsync(dto.PropertyId);
        if (property == null)
            return (null, $"Объект недвижимости с ID {dto.PropertyId} не найден");

        var request = new Request
        {
            Id = Guid.Empty,
            CounterpartyId = counterparty.Id,
            Counterparty = counterparty,
            PropertyId = property.Id,
            Property = property,
            Type = dto.Type,
            Amount = dto.Amount,
            Date = dto.Date
        };

        var created = await requestRepository.AddAsync(request);
        return (mapper.Map<RequestDto>(created), null);
    }

    /// <inheritdoc />
    public async Task<(RequestDto? Result, string? Error)> UpdateAsync(Guid id, UpdateRequestDto dto)
    {
        var existingRequest = await requestRepository.GetByIdAsync(id);
        if (existingRequest == null)
            return (null, $"Заявка с ID {id} не найдена");

        var counterparty = await counterpartyRepository.GetByIdAsync(dto.CounterpartyId);
        if (counterparty == null)
            return (null, $"Контрагент с ID {dto.CounterpartyId} не найден");

        var property = await propertyRepository.GetByIdAsync(dto.PropertyId);
        if (property == null)
            return (null, $"Объект недвижимости с ID {dto.PropertyId} не найден");

        var request = new Request
        {
            Id = id,
            CounterpartyId = counterparty.Id,
            Counterparty = counterparty,
            PropertyId = property.Id,
            Property = property,
            Type = dto.Type,
            Amount = dto.Amount,
            Date = dto.Date
        };

        var updated = await requestRepository.UpdateAsync(id, request);
        return (mapper.Map<RequestDto>(updated), null);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await requestRepository.DeleteAsync(id);
    }
}
