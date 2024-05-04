using TangoSchool.DataAccess.Enums;

namespace TangoSchool.DataAccess.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    public DateTime Timestamp { get; set; }

    public AuditLogType AuditLogType { get; set; }

    public Guid ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;
}
