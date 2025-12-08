using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.WebApi.DTOs;
using RealEstateAgency.WebApi.Repositories;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с объектами недвижимости
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IRealEstatePropertyRepository _repository;
    private readonly IMapper _mapper;

    public PropertiesController(IRealEstatePropertyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все объекты недвижимости
    /// </summary>
    /// <returns>Список объектов недвижимости</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RealEstatePropertyDto>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<RealEstatePropertyDto>> GetAll()
    {
        var properties = _repository.GetAll();
        return Ok(_mapper.Map<IEnumerable<RealEstatePropertyDto>>(properties));
    }

    /// <summary>
    /// Получить объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>Объект недвижимости</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RealEstatePropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RealEstatePropertyDto> GetById(int id)
    {
        var property = _repository.GetById(id);
        if (property == null)
            return NotFound($"Объект недвижимости с ID {id} не найден");

        return Ok(_mapper.Map<RealEstatePropertyDto>(property));
    }

    /// <summary>
    /// Создать новый объект недвижимости
    /// </summary>
    /// <param name="dto">Данные объекта</param>
    /// <returns>Созданный объект</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RealEstatePropertyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RealEstatePropertyDto> Create([FromBody] CreateRealEstatePropertyDto dto)
    {
        var property = _mapper.Map<RealEstateProperty>(dto);
        var created = _repository.Add(property);
        var resultDto = _mapper.Map<RealEstatePropertyDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновить объект недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="dto">Новые данные объекта</param>
    /// <returns>Результат операции</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RealEstatePropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RealEstatePropertyDto> Update(int id, [FromBody] UpdateRealEstatePropertyDto dto)
    {
        var property = _mapper.Map<RealEstateProperty>(dto);
        var updated = _repository.Update(id, property);

        if (updated == null)
            return NotFound($"Объект недвижимости с ID {id} не найден");

        return Ok(_mapper.Map<RealEstatePropertyDto>(updated));
    }

    /// <summary>
    /// Удалить объект недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        if (!deleted)
            return NotFound($"Объект недвижимости с ID {id} не найден");

        return NoContent();
    }
}
