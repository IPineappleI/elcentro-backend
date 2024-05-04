using TangoSchool.ApplicationServices.Models.AuditLogs;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface IAuditLogService
{
    public Task<GetAuditLogsResponse> GetAuditLogs
    (
        GetAuditLogsPayload payload,
        CancellationToken cancellationToken
    );

    public Task<GetAuditLogsResponse> GetAuditLogsByIdentityId
    (
        GetAuditLogsByIdentityIdPayload payload,
        CancellationToken cancellationToken
    );
}
