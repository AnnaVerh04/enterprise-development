using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.WebApi.DTOs;
using RealEstateAgency.WebApi.Services;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для аналитических запросов
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начало периода</param>
    /// <param name="endDate">Конец периода</param>
    /// <returns>Список ФИО продавцов</returns>
    [HttpGet("sellers")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> GetSellersInPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var sellers = _analyticsService.GetSellersInPeriod(startDate, endDate);
        return Ok(sellers);
    }

    /// <summary>
    /// Получить топ-5 клиентов по количеству заявок (покупка и продажа отдельно)
    /// </summary>
    /// <returns>Топ-5 покупателей и топ-5 продавцов</returns>
    [HttpGet("top-clients")]
    [ProducesResponseType(typeof(Top5ClientsResultDto), StatusCodes.Status200OK)]
    public ActionResult<Top5ClientsResultDto> GetTop5Clients()
    {
        var result = _analyticsService.GetTop5ClientsByRequestCount();
        return Ok(result);
    }

    /// <summary>
    /// Получить статистику заявок по типам недвижимости
    /// </summary>
    /// <returns>Количество заявок по каждому типу недвижимости</returns>
    [HttpGet("property-type-statistics")]
    [ProducesResponseType(typeof(IEnumerable<PropertyTypeStatisticsDto>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<PropertyTypeStatisticsDto>> GetPropertyTypeStatistics()
    {
        var statistics = _analyticsService.GetRequestCountByPropertyType();
        return Ok(statistics);
    }

    /// <summary>
    /// Получить клиентов с заявками минимальной стоимости
    /// </summary>
    /// <returns>Информация о клиентах с минимальной суммой</returns>
    [HttpGet("min-amount-clients")]
    [ProducesResponseType(typeof(ClientWithMinAmountDto), StatusCodes.Status200OK)]
    public ActionResult<ClientWithMinAmountDto> GetClientsWithMinAmount()
    {
        var result = _analyticsService.GetClientsWithMinAmount();
        return Ok(result);
    }

    /// <summary>
    /// Получить клиентов, ищущих определённый тип недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости</param>
    /// <returns>Список ФИО клиентов</returns>
    [HttpGet("clients-by-property-type")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> GetClientsByPropertyType(
        [FromQuery] PropertyType propertyType)
    {
        var clients = _analyticsService.GetClientsSeekingPropertyType(propertyType);
        return Ok(clients);
    }
}
