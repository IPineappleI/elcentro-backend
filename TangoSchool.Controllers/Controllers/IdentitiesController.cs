using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangoSchool.ApplicationServices.Models.Identities;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.Extensions;

namespace TangoSchool.Controllers;

/// <summary>
/// Контроллер идентификации предоставляет API для аутентификации пользователей и управления их учетными данными.
/// </summary>
[ApiController]
[Route("Identities")]
public class IdentitiesController : ControllerBase
{
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Создает экземпляр контроллера IdentitiesController с указанным сервисом идентификации.
    /// </summary>
    /// <param name="identityService">Сервис идентификации.</param>
    public IdentitiesController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    /// <param name="request">Запрос на аутентификацию.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Ответ с токеном аутентификации.</returns>
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate
    (
        [FromBody] AuthRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await _identityService.Authenticate(request, cancellationToken);
    }

    /// <summary>
    /// Получение информации о пользователе.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Информация о пользователе.</returns>
    [Authorize]
    [HttpGet("UserInformation")]
    public async Task<ActionResult<UserInformationWithRoles>> GetUserInformation
    (
        CancellationToken cancellationToken
    )
    {
        return await _identityService.GetUserInformation(User.GetUserId(), cancellationToken);
    }

    /// <summary>
    /// Обновление информации о пользователе.
    /// </summary>
    /// <param name="request">Запрос на обновление информации о пользователе.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат операции.</returns>
    [Authorize]
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser
    (
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid) return BadRequest(request);

        await _identityService.UpdateUser(User.GetUserId(), request, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Изменение пароля пользователя.
    /// </summary>
    /// <param name="request">Запрос на изменение пароля.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат операции.</returns>
    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword
    (
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid) return BadRequest(request);

        await _identityService.ChangePassword(User.GetUserId(), request, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Отзыв токена доступа.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат операции.</returns>
    [Authorize]
    [HttpPost]
    [Route("RevokeToken")]
    public async Task<IActionResult> RevokeToken
    (
        CancellationToken cancellationToken
    )
    {
        await _identityService.RevokeToken(User.GetUserId(), cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновление токена доступа.
    /// </summary>
    /// <param name="tokenModel">Модель токена для обновления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Модель обновленного токена.</returns>
    [HttpPost]
    [Route("RefreshToken")]
    public async Task<ActionResult<TokenModel>> RefreshToken
    (
        TokenModel tokenModel,
        CancellationToken cancellationToken
    )
    {
        return await _identityService.RefreshToken(tokenModel, cancellationToken);
    }

    /// <summary>
    /// Запрос на сброс пароля.
    /// </summary>
    /// <param name="payload">Данные для сброса пароля.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат операции.</returns>
    [HttpPost]
    [Route("ResetPassword/Request")]
    public async Task<ActionResult> RequestResetPassword
    (
        RequestResetPasswordPayload payload,
        CancellationToken cancellationToken
    )
    {
        await _identityService.RequestResetPassword(payload, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Подтверждение сброса пароля.
    /// </summary>
    /// <param name="payload">Данные для подтверждения сброса пароля.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Модель обновленного токена.</returns>
    [HttpPost]
    [Route("ResetPassword/Confirm")]
    public async Task<ActionResult<TokenModel>> ConfirmResetPassword
    (
        ConfirmResetPasswordPayload payload,
        CancellationToken cancellationToken
    )
    {
        await _identityService.ConfirmResetPassword(payload, cancellationToken);

        return Ok();
    }
}
