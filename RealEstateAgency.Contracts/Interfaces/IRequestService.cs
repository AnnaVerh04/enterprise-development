using RealEstateAgency.Contracts.Dto;

namespace RealEstateAgency.Contracts.Interfaces;

/// <summary>
/// Интерфейс сервиса заявок
/// </summary>
public interface IRequestService
{
    /// <summary>
    /// Получить все заявки
    /// </summary>
    public Task<IEnumerable<RequestDto>> GetAllAsync();

    /// <summary>
    /// Получить заявку по идентификатору
    /// </summary>
    public Task<RequestDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Создать заявку
    /// </summary>
    /// <returns>Созданная заявка или null если контрагент или недвижимость не найдены</returns>
    public Task<(RequestDto? Result, string? Error)> CreateAsync(CreateRequestDto dto);

    /// <summary>
    /// Обновить заявку
    /// </summary>
    /// <returns>Обновленная заявка или null если не найдена</returns>
    public Task<(RequestDto? Result, string? Error)> UpdateAsync(Guid id, UpdateRequestDto dto);

    /// <summary>
    /// Удалить заявку
    /// </summary>
    public Task<bool> DeleteAsync(Guid id);
}
