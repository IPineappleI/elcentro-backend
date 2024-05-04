using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Models.AuditLogs;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class AuditLogService : IAuditLogService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;

    public AuditLogService(IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext)
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
    }

    public async Task<GetAuditLogsResponse> GetAuditLogs
    (
        GetAuditLogsPayload payload,
        CancellationToken cancellationToken
    )
    {
        var query = _readOnlyTangoSchoolDbContext.AuditLogs.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .OrderBy(x => x.Timestamp)
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new AuditLogModel
            (
                x.ApplicationUserId,
                x.ApplicationUser.FirstName,
                x.Timestamp,
                x.AuditLogType
            ))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }

    public async Task<GetAuditLogsResponse> GetAuditLogsByIdentityId
    (
        GetAuditLogsByIdentityIdPayload payload,
        CancellationToken cancellationToken
    )
    {
        var query = _readOnlyTangoSchoolDbContext.AuditLogs
            .Where(x => x.ApplicationUserId == payload.UserId);

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .OrderBy(x => x.Timestamp)
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x =>
                new AuditLogModel
                (
                    x.ApplicationUserId,
                    x.ApplicationUser.FirstName,
                    x.Timestamp,
                    x.AuditLogType
                ))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }
}
