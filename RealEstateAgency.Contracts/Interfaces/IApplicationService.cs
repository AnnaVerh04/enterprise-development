namespace RealEstateAgency.Contracts.Interfaces;

/// <summary>
/// Базовый интерфейс сервиса приложения
/// </summary>
/// <typeparam name="TDto">DTO для отображения</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO для создания/обновления</typeparam>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
{
    /// <summary>
    /// Получить все сущности
    /// </summary>
    public Task<IEnumerable<TDto>> GetAllAsync();

    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    public Task<TDto?> GetByIdAsync(TKey id);

    /// <summary>
    /// Создать сущность
    /// </summary>
    public Task<TDto> CreateAsync(TCreateUpdateDto dto);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    public Task<TDto?> UpdateAsync(TKey id, TCreateUpdateDto dto);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    public Task<bool> DeleteAsync(TKey id);
}
