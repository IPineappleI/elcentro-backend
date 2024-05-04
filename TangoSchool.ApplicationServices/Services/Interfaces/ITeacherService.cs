using TangoSchool.ApplicationServices.Models.Teachers;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface ITeacherService
{
    Task<List<TeacherHeader>> GetTeacherHeaders
    (
        CancellationToken cancellationToken
    );
    
    Task<GetCurrentTeacherGroupsResponse> GetCurrentTeacherGroups
    (
        Guid userId,
        GetCurrentTeacherGroupsPayload payload,
        CancellationToken cancellationToken
    );
}
