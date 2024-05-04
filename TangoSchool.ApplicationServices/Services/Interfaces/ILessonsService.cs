using TangoSchool.ApplicationServices.Models.Lessons;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface ILessonsService
{
    Task<LessonsMetadata> GetLessonsMetadata
    (
        CancellationToken cancellationToken
    );

    Task<Guid> CreateLesson
    (
        CreateLessonPayload payload,
        CancellationToken cancellationToken
    );

    Task UpdateLesson
    (
        UpdateLesson payload,
        CancellationToken cancellationToken
    );

    Task<GetLessonResponse> GetLesson
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task<GetAllLessonsResponse> GetAllLessons
    (
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    );

    Task<GetAllLessonsResponse> GetAllLessonsByStudent
    (
        Guid userId,
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    );

    Task<GetAllLessonsResponse> GetAllLessonsByTeacher
    (
        Guid userId,
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    );

    Task SetLessonAttendance
    (
        Guid id,
        SetLessonAttendancePayload payload,
        CancellationToken cancellationToken
    );

    Task TerminateLesson
    (
        Guid id,
        CancellationToken cancellationToken
    );
    
    Task RestoreLesson
    (
        Guid id,
        CancellationToken cancellationToken
    );
}
