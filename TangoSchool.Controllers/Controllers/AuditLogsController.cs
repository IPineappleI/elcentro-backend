using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.AuditLogs;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для работы с журналом аудита.
/// </summary>
[ApiController]
[Authorize(Roles = RoleConstants.Administrator, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("AuditLogs")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _logService;

    /// <summary>
    /// Создает экземпляр контроллера AuditLogsController с указанным сервисом журнала аудита.
    /// </summary>
    /// <param name="logService">Сервис журнала аудита.</param>
    public AuditLogsController(IAuditLogService logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Получает все записи из журнала аудита.
    /// </summary>
    /// <param name="payload">Параметры запроса для пагинации записей журнала аудита.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения запроса с записями журнала аудита.</returns>
    [HttpGet("All")]
    public async Task<ActionResult<GetAuditLogsResponse>> GetAuditLogs
    (
        [FromQuery] GetAuditLogsPayload payload,
        CancellationToken cancellationToken
    )
    {
        return await _logService.GetAuditLogs(payload, cancellationToken);
    }

    /// <summary>
    /// Получает записи из журнала аудита по идентификатору пользователя.
    /// </summary>
    /// <param name="payload">Параметры запроса для фильтрации записей журнала аудита по идентификатору пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения запроса с записями журнала аудита по идентификатору пользователя.</returns>
    [HttpGet("ByUserId")]
    public async Task<ActionResult<GetAuditLogsResponse>> GetAuditLogsByIdentityId
    (
        [FromQuery] GetAuditLogsByIdentityIdPayload payload,
        CancellationToken cancellationToken
    )
    {
        return await _logService.GetAuditLogsByIdentityId(payload, cancellationToken);
    }
}
