using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.WebApi.DTOs;
using RealEstateAgency.WebApi.Repositories;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с заявками
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly IRequestRepository _requestRepository;
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly IRealEstatePropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public RequestsController(
        IRequestRepository requestRepository,
        ICounterpartyRepository counterpartyRepository,
        IRealEstatePropertyRepository propertyRepository,
        IMapper mapper)
    {
        _requestRepository = requestRepository;
        _counterpartyRepository = counterpartyRepository;
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все заявки
    /// </summary>
    /// <returns>Список заявок</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RequestDto>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<RequestDto>> GetAll()
    {
        var requests = _requestRepository.GetAll();
        return Ok(_mapper.Map<IEnumerable<RequestDto>>(requests));
    }

    /// <summary>
    /// Получить заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>Заявка</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RequestDto> GetById(int id)
    {
        var request = _requestRepository.GetById(id);
        if (request == null)
            return NotFound($"Заявка с ID {id} не найдена");

        return Ok(_mapper.Map<RequestDto>(request));
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
    public ActionResult<RequestDto> Create([FromBody] CreateRequestDto dto)
    {
        var counterparty = _counterpartyRepository.GetById(dto.CounterpartyId);
        if (counterparty == null)
            return NotFound($"Контрагент с ID {dto.CounterpartyId} не найден");

        var property = _propertyRepository.GetById(dto.PropertyId);
        if (property == null)
            return NotFound($"Объект недвижимости с ID {dto.PropertyId} не найден");

        var request = new Request
        {
            Id = 0,
            Counterparty = counterparty,
            Property = property,
            Type = dto.Type,
            Amount = dto.Amount,
            Date = dto.Date
        };

        var created = _requestRepository.Add(request);
        var resultDto = _mapper.Map<RequestDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновить заявку
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="dto">Новые данные заявки</param>
    /// <returns>Результат операции</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RequestDto> Update(int id, [FromBody] UpdateRequestDto dto)
    {
        var existingRequest = _requestRepository.GetById(id);
        if (existingRequest == null)
            return NotFound($"Заявка с ID {id} не найдена");

        var counterparty = _counterpartyRepository.GetById(dto.CounterpartyId);
        if (counterparty == null)
            return NotFound($"Контрагент с ID {dto.CounterpartyId} не найден");

        var property = _propertyRepository.GetById(dto.PropertyId);
        if (property == null)
            return NotFound($"Объект недвижимости с ID {dto.PropertyId} не найден");

        var request = new Request
        {
            Id = id,
            Counterparty = counterparty,
            Property = property,
            Type = dto.Type,
            Amount = dto.Amount,
            Date = dto.Date
        };

        var updated = _requestRepository.Update(id, request);
        return Ok(_mapper.Map<RequestDto>(updated));
    }

    /// <summary>
    /// Удалить заявку
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _requestRepository.Delete(id);
        if (!deleted)
            return NotFound($"Заявка с ID {id} не найдена");

        return NoContent();
    }
}
