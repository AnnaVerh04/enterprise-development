using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с контрагентами
/// </summary>
public class CounterpartiesController(
    ICounterpartyService service,
    ILogger<CounterpartiesController> logger)
    : BaseCrudController<CounterpartyDto, CreateCounterpartyDto, UpdateCounterpartyDto, ICounterpartyService>(service, logger)
{
    /// <inheritdoc />
    protected override Guid GetEntityId(CounterpartyDto entity) => entity.Id;

    /// <summary>
    /// Получить всех контрагентов
    /// </summary>
    /// <returns>Список контрагентов</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CounterpartyDto>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<IEnumerable<CounterpartyDto>>> GetAll()
    {
        Logger.LogInformation("Запрос на получение всех контрагентов");
        var counterparties = await Service.GetAllAsync();
        Logger.LogInformation("Возвращено {Count} контрагентов", counterparties.Count());
        return Ok(counterparties);
    }

    /// <summary>
    /// Получить контрагента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <returns>Контрагент</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CounterpartyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override async Task<ActionResult<CounterpartyDto>> GetById(Guid id)
    {
        Logger.LogInformation("Запрос на получение контрагента с ID {Id}", id);
        var counterparty = await Service.GetByIdAsync(id);
        if (counterparty == null)
        {
            Logger.LogWarning("Контрагент с ID {Id} не найден", id);
            return NotFound($"Контрагент с ID {id} не найден");
        }

        return Ok(counterparty);
    }

    /// <summary>
    /// Создать нового контрагента
    /// </summary>
    /// <param name="dto">Данные контрагента</param>
    /// <returns>Созданный контрагент</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CounterpartyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<CounterpartyDto>> Create([FromBody] CreateCounterpartyDto dto)
    {
        if (!ModelState.IsValid)
        {
            Logger.LogWarning("Ошибка валидации при создании контрагента: {Errors}", ModelState);
            return BadRequest(ModelState);
        }

        Logger.LogInformation("Создание контрагента: {FullName}", dto.FullName);
        var created = await Service.CreateAsync(dto);
        Logger.LogInformation("Контрагент создан с ID {Id}", created.Id);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить контрагента
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <param name="dto">Новые данные контрагента</param>
    /// <returns>Результат операции</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CounterpartyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CounterpartyDto>> Update(Guid id, [FromBody] UpdateCounterpartyDto dto)
    {
        if (!ModelState.IsValid)
        {
            Logger.LogWarning("Ошибка валидации при обновлении контрагента {Id}: {Errors}", id, ModelState);
            return BadRequest(ModelState);
        }

        Logger.LogInformation("Обновление контрагента с ID {Id}", id);
        var updated = await Service.UpdateAsync(id, dto);

        if (updated == null)
        {
            Logger.LogWarning("Контрагент с ID {Id} не найден для обновления", id);
            return NotFound($"Контрагент с ID {id} не найден");
        }

        Logger.LogInformation("Контрагент с ID {Id} успешно обновлен", id);
        return Ok(updated);
    }

    /// <summary>
    /// Удалить контрагента
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override async Task<IActionResult> Delete(Guid id)
    {
        Logger.LogInformation("Удаление контрагента с ID {Id}", id);
        var deleted = await Service.DeleteAsync(id);
        if (!deleted)
        {
            Logger.LogWarning("Контрагент с ID {Id} не найден для удаления", id);
            return NotFound($"Контрагент с ID {id} не найден");
        }

        Logger.LogInformation("Контрагент с ID {Id} успешно удален", id);
        return NoContent();
    }
}
