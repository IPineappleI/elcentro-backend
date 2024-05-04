using Microsoft.AspNetCore.Identity;

namespace TangoSchool.DataAccess.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }

    public string? Photo { get; set; }

    public string? Description { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public List<AuditLog> AuditLogs { get; set; } = new();
}
