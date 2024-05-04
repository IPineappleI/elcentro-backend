using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для управления классами в школе.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("Classrooms")]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomsService _classroomsService;

    /// <summary>
    /// Создает экземпляр контроллера ClassroomsController с указанным сервисом классов.
    /// </summary>
    /// <param name="classroomsService">Сервис классов.</param>
    public ClassroomsController(IClassroomsService classroomsService)
    {
        _classroomsService = classroomsService;
    }
    
    /// <summary>
    /// Создает новый класс.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> CreateClassroom
    (
        [FromBody] CreateClassroomPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _classroomsService.CreateClassroom(payload, cancellationToken));
    }

    /// <summary>
    /// Обновляет информацию о классе.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateClassroom
    (
        [FromBody] UpdateClassroom payload,
        CancellationToken cancellationToken
    )
    {
        await _classroomsService.UpdateClassroom(payload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Получает информацию о классе по его идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetClassroomResponse>> GetClassroom
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _classroomsService.GetClassroom(id, cancellationToken));
    }

    /// <summary>
    /// Помечет класс удаленным по его идентификатору.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("{id:guid}/Terminate")]
    public async Task<ActionResult> TerminateClassroom
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _classroomsService.TerminateClassroom(id, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Восстанавливает помеченный удаленным класс по его идентификатору.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("{id:guid}/Restore")]
    public async Task<ActionResult> RestoreClassroom
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _classroomsService.RestoreClassroom(id, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Получает информацию о всех классах.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Administrator}")]
    [HttpGet("All")]
    public async Task<ActionResult<GetAllClassroomsResponse>> GetAllClassrooms
    (
        [FromQuery] GetAllClassroomsPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _classroomsService.GetAllClassrooms(payload, cancellationToken));
    }

    /// <summary>
    /// Получает список доступных классов для учителя или администратора.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Administrator}")]
    [HttpGet("AvailableClassrooms")]
    public async Task<ActionResult<List<ClassroomHeader>>> GetAvailableClassrooms
    (
        [FromQuery] GetAvailableClassroomsPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _classroomsService.GetAvailableClassrooms(payload, cancellationToken));
    }

    /// <summary>
    /// Получает заголовки и идентификаторы всех классов.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Administrator}")]
    [HttpGet("Headers")]
    public async Task<ActionResult<List<ClassroomHeader>>> GetClassroomHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return Ok(await _classroomsService.GetClassroomHeaders(cancellationToken));
    }
}
