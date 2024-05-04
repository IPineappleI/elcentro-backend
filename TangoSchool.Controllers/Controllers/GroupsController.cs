using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.Groups;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для работы с группами.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("Groups")]
public class GroupsController : ControllerBase
{
    private readonly IGroupsService _groupsService;

    /// <summary>
    /// Создает экземпляр контроллера GroupsController с указанным сервисом групп.
    /// </summary>
    /// <param name="groupsService">Сервис групп.</param>
    public GroupsController(IGroupsService groupsService)
    {
        _groupsService = groupsService;
    }

    /// <summary>
    /// Получает метаданные о группах.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Administrator}")]
    [HttpPost("Metadata")]
    public async Task<ActionResult<GroupsMetadata>> GetGroupsMetadata
    (
        CancellationToken cancellationToken
    )
    {
        return Ok(await _groupsService.GetGroupsMetadata(cancellationToken));
    }

    /// <summary>
    /// Создает новую группу.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> CreateGroup
    (
        [FromBody] CreateGroupPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _groupsService.CreateGroup(payload, cancellationToken));
    }

    /// <summary>
    /// Обновляет информацию о группе.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateGroup
    (
        [FromBody] UpdateGroupPayload payload,
        CancellationToken cancellationToken
    )
    {
        await _groupsService.UpdateGroup(payload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Получает информацию о группе по её идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetGroupResponse>> GetGroup
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _groupsService.GetGroup(id, cancellationToken));
    }

    /// <summary>
    /// Получает информацию о всех группах.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Administrator}")]
    [HttpGet("All")]
    public async Task<ActionResult<GetAllGroupsResponse>> GetAllGroups
    (
        [FromQuery] GetAllGroupsPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _groupsService.GetAllGroups(payload, cancellationToken));
    }

    /// <summary>
    /// Получает основную информацию и идентификаторы всех групп.
    /// </summary>
    [Authorize(Roles = $"{RoleConstants.Teacher},{RoleConstants.Administrator}")]
    [HttpGet("Headers")]
    public async Task<ActionResult<List<GroupHeader>>> GetGroupHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return Ok(await _groupsService.GetGroupHeaders(cancellationToken));
    }

    /// <summary>
    /// Помечает группу удаленной по её идентификатору.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("{id:guid}/Terminate")]
    public async Task<ActionResult> TerminateGroup
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _groupsService.TerminateGroup(id, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Восстанавливает помеченную удаленной группу по её идентификатору.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("{id:guid}/Restore")]
    public async Task<ActionResult> RestoreGroup
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _groupsService.RestoreGroup(id, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Добавляет студента в группу.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("{id:guid}/Students/Add/{studentId:guid}")]
    public async Task<ActionResult> AddStudentToGroup
    (
        [FromRoute] Guid id,
        [FromRoute] Guid studentId,
        CancellationToken cancellationToken
    )
    {
        await _groupsService.AddStudentToGroup(id, studentId, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Удаляет студента из группы.
    /// </summary>
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpDelete("{id:guid}/Students/Remove/{studentId:guid}")]
    public async Task<ActionResult> RemoveStudentFromGroup
    (
        [FromRoute] Guid id,
        [FromRoute] Guid studentId,
        CancellationToken cancellationToken
    )
    {
        await _groupsService.RemoveStudentFromGroup(id, studentId, cancellationToken);

        return Ok();
    }
}
