using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Contracts.Interfaces;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Базовый контроллер для CRUD операций
/// </summary>
/// <typeparam name="TDto">DTO для отображения</typeparam>
/// <typeparam name="TCreateDto">DTO для создания</typeparam>
/// <typeparam name="TUpdateDto">DTO для обновления</typeparam>
/// <typeparam name="TService">Тип сервиса</typeparam>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseCrudController<TDto, TCreateDto, TUpdateDto, TService>(
    TService service,
    ILogger logger) : ControllerBase
    where TService : IApplicationService<TDto, TCreateDto, Guid>
{
    protected readonly TService Service = service;
    protected readonly ILogger Logger = logger;

    /// <summary>
    /// Получить все сущности
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        try
        {
            Logger.LogInformation("Запрос на получение всех сущностей");
            var entities = await Service.GetAllAsync();
            Logger.LogInformation("Возвращено {Count} сущностей", entities.Count());
            return Ok(entities);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при получении всех сущностей");
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> GetById(Guid id)
    {
        try
        {
            Logger.LogInformation("Запрос на получение сущности с ID {Id}", id);
            var entity = await Service.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogWarning("Сущность с ID {Id} не найдена", id);
                return NotFound($"Сущность с ID {id} не найдена");
            }

            return Ok(entity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при получении сущности с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Создать новую сущность
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogWarning("Ошибка валидации при создании сущности: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            Logger.LogInformation("Создание сущности");
            var created = await Service.CreateAsync(dto);
            Logger.LogInformation("Сущность создана");

            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(created) }, created);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при создании сущности");
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Удалить сущность
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            Logger.LogInformation("Удаление сущности с ID {Id}", id);
            var deleted = await Service.DeleteAsync(id);
            if (!deleted)
            {
                Logger.LogWarning("Сущность с ID {Id} не найдена для удаления", id);
                return NotFound($"Сущность с ID {id} не найдена");
            }

            Logger.LogInformation("Сущность с ID {Id} успешно удалена", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при удалении сущности с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Получить идентификатор сущности из DTO
    /// </summary>
    protected abstract Guid GetEntityId(TDto entity);
}
