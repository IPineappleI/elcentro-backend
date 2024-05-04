using TangoSchool.ApplicationServices.Models.Classrooms;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface IClassroomsService
{
    Task<Guid> CreateClassroom
    (
        CreateClassroomPayload payload,
        CancellationToken cancellationToken
    );

    Task UpdateClassroom
    (
        UpdateClassroom payload,
        CancellationToken cancellationToken
    );

    Task<GetClassroomResponse> GetClassroom
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task TerminateClassroom
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task RestoreClassroom
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task<GetAllClassroomsResponse> GetAllClassrooms
    (
        GetAllClassroomsPayload payload,
        CancellationToken cancellationToken
    );

    Task<List<ClassroomHeader>> GetAvailableClassrooms
    (
        GetAvailableClassroomsPayload payload,
        CancellationToken cancellationToken
    );
    
    Task<List<ClassroomHeader>> GetClassroomHeaders
    (
        CancellationToken cancellationToken
    );
}
