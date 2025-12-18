using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Contracts.Interfaces;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.WebApi.Controllers;

/// <summary>
/// Контроллер для аналитических запросов
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начало периода</param>
    /// <param name="endDate">Конец периода</param>
    /// <returns>Список ФИО продавцов</returns>
    [HttpGet("sellers")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetSellersInPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        logger.LogInformation("Запрос продавцов за период {StartDate} - {EndDate}", startDate, endDate);
        var sellers = await analyticsService.GetSellersInPeriodAsync(startDate, endDate);
        logger.LogInformation("Найдено {Count} продавцов", sellers.Count());
        return Ok(sellers);
    }

    /// <summary>
    /// Получить топ-5 клиентов по количеству заявок (покупка и продажа отдельно)
    /// </summary>
    /// <returns>Топ-5 покупателей и топ-5 продавцов</returns>
    [HttpGet("top-clients")]
    [ProducesResponseType(typeof(Top5ClientsResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<Top5ClientsResultDto>> GetTop5Clients()
    {
        logger.LogInformation("Запрос топ-5 клиентов");
        var result = await analyticsService.GetTop5ClientsByRequestCountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить статистику заявок по типам недвижимости
    /// </summary>
    /// <returns>Количество заявок по каждому типу недвижимости</returns>
    [HttpGet("property-type-statistics")]
    [ProducesResponseType(typeof(IEnumerable<PropertyTypeStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PropertyTypeStatisticsDto>>> GetPropertyTypeStatistics()
    {
        logger.LogInformation("Запрос статистики по типам недвижимости");
        var statistics = await analyticsService.GetRequestCountByPropertyTypeAsync();
        return Ok(statistics);
    }

    /// <summary>
    /// Получить клиентов с заявками минимальной стоимости
    /// </summary>
    /// <returns>Информация о клиентах с минимальной суммой</returns>
    [HttpGet("min-amount-clients")]
    [ProducesResponseType(typeof(ClientWithMinAmountDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ClientWithMinAmountDto>> GetClientsWithMinAmount()
    {
        logger.LogInformation("Запрос клиентов с минимальной суммой заявки");
        var result = await analyticsService.GetClientsWithMinAmountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получить клиентов, ищущих определённый тип недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости</param>
    /// <returns>Список ФИО клиентов</returns>
    [HttpGet("clients-by-property-type")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetClientsByPropertyType(
        [FromQuery] PropertyType propertyType)
    {
        logger.LogInformation("Запрос клиентов, ищущих недвижимость типа {PropertyType}", propertyType);
        var clients = await analyticsService.GetClientsSeekingPropertyTypeAsync(propertyType);
        logger.LogInformation("Найдено {Count} клиентов", clients.Count());
        return Ok(clients);
    }
}
