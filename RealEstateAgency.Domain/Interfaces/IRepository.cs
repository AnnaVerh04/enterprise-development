namespace RealEstateAgency.Domain.Interfaces;

/// <summary>
/// Базовый интерфейс репозитория
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Получить все сущности
    /// </summary>
    public Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    public Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Добавить сущность
    /// </summary>
    public Task<T> AddAsync(T entity);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    public Task<T?> UpdateAsync(Guid id, T entity);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    public Task<bool> DeleteAsync(Guid id);
}
