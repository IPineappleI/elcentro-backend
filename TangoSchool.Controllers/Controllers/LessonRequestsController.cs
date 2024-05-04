using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.LessonRequests;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.Extensions;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер заявок на уроки и управления ими.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("LessonRequest")]
public class LessonRequestsController : ControllerBase
{
    private readonly ILessonRequestsService _lessonRequestsService;

    /// <summary>
    /// Создает экземпляр контроллера LessonRequestsController с указанным сервисом заявок на уроки.
    /// </summary>
    /// <param name="lessonRequestsService">Сервис заявок на уроки.</param>
    public LessonRequestsController(ILessonRequestsService lessonRequestsService)
    {
        _lessonRequestsService = lessonRequestsService;
    }

    /// <summary>
    /// Создает новую заявку на урок с указанными параметрами.
    /// </summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> CreateLessonRequest
    (
        [FromBody] CreateLessonRequestPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _lessonRequestsService.CreateLessonRequest(User.GetUserId(), payload, cancellationToken));
    }

    /// <summary>
    /// Отклоняет заявку на урок по ее идентификатору.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Student}")]
    [HttpPost("{id:guid}/Reject")]
    public async Task<ActionResult> RejectLessonRequest
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _lessonRequestsService.RejectLessonRequest(User.GetUserId(), id, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Утверждает заявку на урок по ее идентификатору.
    /// </summary>
    [Authorize(Roles = RoleConstants.Teacher)]
    [HttpPost("{id:guid}/Approve")]
    public async Task<ActionResult> RejectLessonRequest
    (
        [FromRoute] Guid id,
        [FromBody] ApproveLessonRequestPayload payload,
        CancellationToken cancellationToken
    )
    {
        await _lessonRequestsService.ApproveLessonRequest(User.GetUserId(), id, payload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Получает все заявки на урок, отправленные учителем.
    /// </summary>
    [Authorize(Roles = RoleConstants.Teacher)]
    [HttpGet("AllByTeacher")]
    public async Task<ActionResult<GetLessonRequestByTeacherResponse>> GetLessonRequestsByTeacher
    (
        [FromQuery] GetLessonRequestByTeacherPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _lessonRequestsService.GetLessonRequestsByTeacher(
            User.GetUserId(), payload, cancellationToken));
    }

    /// <summary>
    /// Получает все заявки на урок, отправленные студентом.
    /// </summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpGet("AllByStudent")]
    public async Task<ActionResult<GetLessonRequestByTeacherResponse>> GetLessonRequestsByStudent
    (
        [FromQuery] GetLessonRequestByTeacherPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _lessonRequestsService.GetLessonRequestsByStudent(
            User.GetUserId(), payload, cancellationToken));
    }
}
