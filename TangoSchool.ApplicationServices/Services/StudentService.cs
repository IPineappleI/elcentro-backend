using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

public class StudentService : IStudentService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;

    public StudentService(IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext)
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
    }
    
    public async Task<List<StudentHeader>> GetStudentHeaders(CancellationToken cancellationToken)
    {
        return await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Select(x => new StudentHeader
            (
                x.Id,
                x.ApplicationUser.FirstName,
                x.ApplicationUser.LastName,
                x.ApplicationUser.MiddleName
            ))
            .ToListAsync(cancellationToken);
    }
}
