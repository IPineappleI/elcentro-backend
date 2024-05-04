using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class TeacherService : ITeacherService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;

    public TeacherService(IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext)
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
    }

    public async Task<List<TeacherHeader>> GetTeacherHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return await _readOnlyTangoSchoolDbContext
            .Teachers
            .FilterActive()
            .Select(x => new TeacherHeader
            (
                x.Id,
                x.ApplicationUser.FirstName,
                x.ApplicationUser.LastName,
                x.ApplicationUser.MiddleName
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<GetCurrentTeacherGroupsResponse> GetCurrentTeacherGroups
    (
        Guid userId,
        GetCurrentTeacherGroupsPayload payload,
        CancellationToken cancellationToken
    )
    {
        var query = _readOnlyTangoSchoolDbContext.Groups.AsQueryable();

        if (!payload.IncludeTerminated)
        {
            query = query.FilterActive();
        }

        query = query.Where(x => x.Teacher.ApplicationUserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Select(x => new GetCurrentTeacherGroupsResponseItems
            (
                x.Id,
                x.Name,
                x.Description,
                x.Level,
                x.JoinedStudentGroups.Count,
                x.MaxStudentCapacity,
                new
                (
                    x.TeacherId,
                    x.Teacher.ApplicationUser.FirstName,
                    x.Teacher.ApplicationUser.LastName,
                    x.Teacher.ApplicationUser.MiddleName
                ),
                x.Terminated
            ))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return new(items, totalCount);
    }
}
