using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.Events;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для работы с событиями.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("Events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    /// <summary>
    /// Создает экземпляр контроллера EventsController с указанным сервисом событий.
    /// </summary>
    /// <param name="eventService">Сервис событий.</param>
    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Создает новое событие.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> CreateEvente
    (
        [FromBody] CreateEventPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _eventService.CreateEvent(payload, cancellationToken));
    }

    /// <summary>
    /// Обновляет информацию о событии.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateEvent
    (
        [FromBody] UpdateEventPayload payload,
        CancellationToken cancellationToken
    )
    {
        await _eventService.UpdateEvent(payload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Получает информацию о событии по его идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetEventResponse>> GetEvent
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _eventService.GetEvent(id, cancellationToken));
    }

    /// <summary>
    /// Получает информацию о всех событиях.
    /// </summary>
    [HttpGet("All")]
    public async Task<ActionResult<GetAllEventsResponse>> GetAllEvents
    (
        [FromQuery] GetAllEventsPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _eventService.GetAllEvents(payload, cancellationToken));
    }
}
