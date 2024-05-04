using TangoSchool.ApplicationServices.Models.Students;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface IStudentService
{
    Task<List<StudentHeader>> GetStudentHeaders
    (
        CancellationToken cancellationToken
    );
}
