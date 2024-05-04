namespace TangoSchool.ApplicationServices.Models.AuditLogs;

public record GetAuditLogsByIdentityIdPayload
(
    Guid UserId,
    int Page,
    int ItemsPerPage
);
