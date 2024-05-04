using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.Extensions;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для работы с учителями.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("Teachers")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    
    /// <summary>
    /// Создает новый экземпляр контроллера учителей.
    /// </summary>
    /// <param name="teacherService">Сервис учителей для взаимодействия с данными учителей.</param>
    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    /// <summary>
    /// Получает ФИО учителей и их айдишники.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список заголовков учителей.</returns>
    [HttpGet("Headers")]
    public async Task<ActionResult<List<TeacherHeader>>> GetTeacherHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return Ok(await _teacherService.GetTeacherHeaders(cancellationToken));
    }

    /// <summary>
    /// Получает группы текущего учителя.
    /// </summary>
    /// <param name="payload">Параметры запроса.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Ответ с информацией о группах текущего учителя.</returns>
    [HttpGet("Current/Groups")]
    public async Task<ActionResult<GetCurrentTeacherGroupsResponse>> GetCurrentTeacherGroups
    (
        [FromQuery] GetCurrentTeacherGroupsPayload payload,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _teacherService.GetCurrentTeacherGroups(User.GetUserId(), payload, cancellationToken));
    }
}
