namespace TangoSchool.ApplicationServices.Models.AuditLogs;

public record GetAuditLogsPayload
(
    int Page,
    int ItemsPerPage
);
