using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.WebApi.DTOs;
using RealEstateAgency.WebApi.Repositories;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с контрагентами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CounterpartiesController : ControllerBase
{
    private readonly ICounterpartyRepository _repository;
    private readonly IMapper _mapper;

    public CounterpartiesController(ICounterpartyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить всех контрагентов
    /// </summary>
    /// <returns>Список контрагентов</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CounterpartyDto>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<CounterpartyDto>> GetAll()
    {
        var counterparties = _repository.GetAll();
        return Ok(_mapper.Map<IEnumerable<CounterpartyDto>>(counterparties));
    }

    /// <summary>
    /// Получить контрагента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <returns>Контрагент</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CounterpartyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CounterpartyDto> GetById(int id)
    {
        var counterparty = _repository.GetById(id);
        if (counterparty == null)
            return NotFound($"Контрагент с ID {id} не найден");

        return Ok(_mapper.Map<CounterpartyDto>(counterparty));
    }

    /// <summary>
    /// Создать нового контрагента
    /// </summary>
    /// <param name="dto">Данные контрагента</param>
    /// <returns>Созданный контрагент</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CounterpartyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CounterpartyDto> Create([FromBody] CreateCounterpartyDto dto)
    {
        var counterparty = _mapper.Map<Counterparty>(dto);
        var created = _repository.Add(counterparty);
        var resultDto = _mapper.Map<CounterpartyDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновить контрагента
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <param name="dto">Новые данные контрагента</param>
    /// <returns>Результат операции</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CounterpartyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CounterpartyDto> Update(int id, [FromBody] UpdateCounterpartyDto dto)
    {
        var counterparty = _mapper.Map<Counterparty>(dto);
        var updated = _repository.Update(id, counterparty);

        if (updated == null)
            return NotFound($"Контрагент с ID {id} не найден");

        return Ok(_mapper.Map<CounterpartyDto>(updated));
    }

    /// <summary>
    /// Удалить контрагента
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        if (!deleted)
            return NotFound($"Контрагент с ID {id} не найден");

        return NoContent();
    }
}
