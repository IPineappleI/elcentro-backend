using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.AuditLogs;

public record AuditLogModel(
    Guid UserId,
    string UserName,
    DateTime Timestamp,
    AuditLogType AuditLogType
);
