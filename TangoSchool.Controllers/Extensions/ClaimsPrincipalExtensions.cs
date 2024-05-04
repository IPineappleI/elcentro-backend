using System.Security.Claims;

namespace TangoSchool.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static string GetUserEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirst(ClaimTypes.Email)?.Value;

        if (!string.IsNullOrWhiteSpace(email))
        {
            return email;
        }

        throw new UnauthorizedAccessException();
    }

    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var idString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(idString, out var id))
        {
            return id;
        }

        throw new UnauthorizedAccessException();
    }
}
