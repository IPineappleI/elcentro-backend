using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для управления студентами.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("Students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    /// <summary>
    /// Создает экземпляр контроллера студентов с указанным сервисом студентов.
    /// </summary>
    /// <param name="studentService">Сервис студентов для использования в контроллере.</param>
    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    /// <summary>
    /// Получение основной информации студентов и их идентификаторов.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список объектов StudentHeader, представляющих информацию о студентах.</returns>
    
    [HttpGet("Headers")]
    public async Task<ActionResult<List<StudentHeader>>> GetStudentHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return Ok(await _studentService.GetStudentHeaders(cancellationToken));
    }
}
