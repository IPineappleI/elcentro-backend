using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Models.Identities;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер для управления администраторами системы.
/// </summary>
[ApiController]
[Route("Admins")]
[Authorize(Roles = RoleConstants.Administrator, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminsController : ControllerBase
{
    private readonly IIdentityService _identityService;
    
    /// <summary>
    /// Инициализирует новый экземпляр контроллера администратора, предоставляя зависимости необходимые для его работы.
    /// </summary>
    /// <param name="identityService">Сервис идентификации, используемый для управления пользователями.</param>
    public AdminsController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Получение информации о пользователе по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    [HttpGet("UserInformation/{id:guid}")]
    public async Task<ActionResult<UserInformationWithRoles>> GetUserInformation
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        return await _identityService.GetUserInformation(id, cancellationToken);
    }

    /// <summary>
    /// Получение списока всех пользователей с пагинацией страниц.
    /// </summary>
    /// <param name="payload">Модель включает себя количество элементов на странице с выдачей и номер страницы. </param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    [HttpGet("AllUsers")]
    public async Task<ActionResult<GetAllUsersResponse>> GetAllUsers
    (
        [FromQuery] GetAllUsersPayload payload,
        CancellationToken cancellationToken
    )
    {
        return await _identityService.GetAllUsers(payload, cancellationToken);
    }


    /// <summary>
    /// Регистрация нового учителя в системе.
    /// </summary>
    /// <param name="userPayload">Модель с информацией для создания учителя.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    /// <returns></returns>
    [HttpPost("RegisterTeacher")]
    public async Task<ActionResult> RegisterTeacher
    (
        [FromBody] RegisterTeacherPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        await _identityService.RegisterTeacher(userPayload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Регистрация нового студента в системе.
    /// </summary>
    /// <param name="userPayload">Модель с информацией для создания студента.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    /// <returns></returns>
    [HttpPost("RegisterStudent")]
    public async Task<ActionResult<AuthResponse>> RegisterStudent
    (
        [FromBody] RegisterStudentPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        await _identityService.RegisterStudent(userPayload, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Регистрация нового администратора в системе.
    /// </summary>
    /// <param name="userPayload">Модель с информацией для создания администратора.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    /// <returns></returns>
    [HttpPost("RegisterAdministrator")]
    public async Task<ActionResult<AuthResponse>> RegisterAdministrator
    (
        [FromBody] RegisterAdministratorPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        await _identityService.RegisterAdministrator(userPayload, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Удаление пользователя из системы по указанному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор удаляемого пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    /// <returns></returns>
    [HttpDelete("Delete/{id:guid}")]
    public async Task<IActionResult> Delete
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _identityService.Delete(id, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновление информации о пользователе в системе по указанному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя.</param>
    /// /// <param name="request">Модель обновления пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    [HttpPut("UpdateUser/{id:guid}")]
    public async Task<IActionResult> UpdateUser
    (
        [FromBody] UpdateUserRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _identityService.UpdateUser(id, request, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Отзыв всех токенов доступа, выпущенных для пользователей системы.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    [HttpPost]
    [Route("RevokeAllTokens")]
    public async Task<IActionResult> RevokeAll
    (
        CancellationToken cancellationToken
    )
    {
        await _identityService.RevokeAll(cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Отзывает все токены доступа и обновления, связанные с указанным пользователем.
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя, чьи токены будут отозваны.</param>
    /// <param name="cancellationToken">Токен отмены операции для асинхронного управления.</param>
    [HttpPost]
    [Route("RevokeToken/{userId:guid}")]
    public async Task<IActionResult> RevokeToken
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        await _identityService.RevokeToken(userId, cancellationToken);

        return Ok();
    }
}
