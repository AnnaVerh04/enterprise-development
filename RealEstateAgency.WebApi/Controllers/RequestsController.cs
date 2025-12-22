using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с заявками
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RequestsController(IRequestService service, ILogger<RequestsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить все заявки
    /// </summary>
    /// <returns>Список заявок</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RequestDto>>> GetAll()
    {
        try
        {
            logger.LogInformation("Запрос на получение всех заявок");
            var requests = await service.GetAllAsync();
            logger.LogInformation("Возвращено {Count} заявок", requests.Count());
            return Ok(requests);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении всех заявок");
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Получить заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>Заявка</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> GetById(Guid id)
    {
        try
        {
            logger.LogInformation("Запрос на получение заявки с ID {Id}", id);
            var request = await service.GetByIdAsync(id);
            if (request == null)
            {
                logger.LogWarning("Заявка с ID {Id} не найдена", id);
                return NotFound($"Заявка с ID {id} не найдена");
            }

            return Ok(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении заявки с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Создать новую заявку
    /// </summary>
    /// <param name="dto">Данные заявки</param>
    /// <returns>Созданная заявка</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> Create([FromBody] CreateRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Ошибка валидации при создании заявки: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            logger.LogInformation("Создание заявки для контрагента {CounterpartyId}", dto.CounterpartyId);
            var (result, error) = await service.CreateAsync(dto);

            if (result == null)
            {
                logger.LogWarning("Ошибка при создании заявки: {Error}", error);
                return NotFound(error);
            }

            logger.LogInformation("Заявка создана с ID {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании заявки");
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Обновить заявку
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="dto">Новые данные заявки</param>
    /// <returns>Результат операции</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> Update(Guid id, [FromBody] UpdateRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Ошибка валидации при обновлении заявки {Id}: {Errors}", id, ModelState);
                return BadRequest(ModelState);
            }

            logger.LogInformation("Обновление заявки с ID {Id}", id);
            var (result, error) = await service.UpdateAsync(id, dto);

            if (result == null)
            {
                logger.LogWarning("Ошибка при обновлении заявки {Id}: {Error}", id, error);
                return NotFound(error);
            }

            logger.LogInformation("Заявка с ID {Id} успешно обновлена", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении заявки с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Удалить заявку
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            logger.LogInformation("Удаление заявки с ID {Id}", id);
            var deleted = await service.DeleteAsync(id);
            if (!deleted)
            {
                logger.LogWarning("Заявка с ID {Id} не найдена для удаления", id);
                return NotFound($"Заявка с ID {id} не найдена");
            }

            logger.LogInformation("Заявка с ID {Id} успешно удалена", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при удалении заявки с ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера");
        }
    }
}
