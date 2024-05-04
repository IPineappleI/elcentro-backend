using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.Subscriptions;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для управления абонементами в системе TangoSchool.
/// </summary>
[ApiController]
[Authorize(Roles = RoleConstants.Administrator, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("Subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsService _subscriptionsService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера SubscriptionsController.
    /// </summary>
    /// <param name="subscriptionsService">Сервис управления абонементами.</param>
    public SubscriptionsController(ISubscriptionsService subscriptionsService)
    {
        _subscriptionsService = subscriptionsService;
    }

    /// <summary>
    /// Получает метаданные о доступных абонементах.
    /// </summary>
    [HttpPost("Metadata")]
    public async Task<ActionResult<SubscriptionMetadata>> GetSubscriptionsMetadata
    (
        CancellationToken cancellationToken
    )
    {
        return Ok(await _subscriptionsService.GetSubscriptionsMetadata(cancellationToken));
    }

    /// <summary>
    /// Создает новый абонемент с указанными параметрами.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> CreateSubscription
    (
        [FromBody] CreateSubscriptionPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _subscriptionsService.CreateSubscription(payload, cancellationToken));
    }

    /// <summary>
    /// Обновляет информацию о существующем абонементе.
    /// </summary>
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateSubscription
    (
        [FromBody] UpdateSubscription payload,
        CancellationToken cancellationToken
    )
    {
        await _subscriptionsService.UpdateSubscription(payload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Возвращает информацию о абонементе по его идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetSubscriptionResponse>> GetSubscription
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _subscriptionsService.GetSubscription(id, cancellationToken));
    }
}
