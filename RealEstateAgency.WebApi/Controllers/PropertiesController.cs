using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с объектами недвижимости
/// </summary>
public class PropertiesController(
    IRealEstatePropertyService service,
    ILogger<PropertiesController> logger)
    : BaseCrudController<RealEstatePropertyDto, CreateRealEstatePropertyDto, UpdateRealEstatePropertyDto, IRealEstatePropertyService>(service, logger)
{
    /// <inheritdoc />
    protected override Guid GetEntityId(RealEstatePropertyDto entity) => entity.Id;

    /// <summary>
    /// Получить все объекты недвижимости
    /// </summary>
    /// <returns>Список объектов недвижимости</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RealEstatePropertyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult<IEnumerable<RealEstatePropertyDto>>> GetAll()
    {
        try
        {
            Logger.LogInformation("Запрос на получение всех объектов недвижимости");
            var properties = await Service.GetAllAsync();
            Logger.LogInformation("Возвращено {Count} объектов недвижимости", properties.Count());
            return Ok(properties);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при получении всех объектов недвижимости");
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Получить объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>Объект недвижимости</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RealEstatePropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult<RealEstatePropertyDto>> GetById(Guid id)
    {
        try
        {
            Logger.LogInformation("Запрос на получение объекта недвижимости с ID {Id}", id);
            var property = await Service.GetByIdAsync(id);
            if (property == null)
            {
                Logger.LogWarning("Объект недвижимости с ID {Id} не найден", id);
                return NotFound($"Объект недвижимости с ID {id} не найден");
            }

            return Ok(property);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при получении объекта недвижимости с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Создать новый объект недвижимости
    /// </summary>
    /// <param name="dto">Данные объекта</param>
    /// <returns>Созданный объект</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RealEstatePropertyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult<RealEstatePropertyDto>> Create([FromBody] CreateRealEstatePropertyDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogWarning("Ошибка валидации при создании объекта недвижимости: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            Logger.LogInformation("Создание объекта недвижимости: {Address}", dto.Address);
            var created = await Service.CreateAsync(dto);
            Logger.LogInformation("Объект недвижимости создан с ID {Id}", created.Id);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при создании объекта недвижимости");
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Обновить объект недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="dto">Новые данные объекта</param>
    /// <returns>Результат операции</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RealEstatePropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RealEstatePropertyDto>> Update(Guid id, [FromBody] UpdateRealEstatePropertyDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogWarning("Ошибка валидации при обновлении объекта недвижимости {Id}: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            Logger.LogInformation("Обновление объекта недвижимости с ID {Id}", id);
            var updated = await Service.UpdateAsync(id, dto);

            if (updated == null)
            {
                Logger.LogWarning("Объект недвижимости с ID {Id} не найден для обновления", id);
                return NotFound($"Объект недвижимости с ID {id} не найден");
            }

            Logger.LogInformation("Объект недвижимости с ID {Id} успешно обновлен", id);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при обновлении объекта недвижимости с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Удалить объект недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            Logger.LogInformation("Удаление объекта недвижимости с ID {Id}", id);
            var deleted = await Service.DeleteAsync(id);
            if (!deleted)
            {
                Logger.LogWarning("Объект недвижимости с ID {Id} не найден для удаления", id);
                return NotFound($"Объект недвижимости с ID {id} не найден");
            }

            Logger.LogInformation("Объект недвижимости с ID {Id} успешно удален", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Ошибка при удалении объекта недвижимости с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }
}
