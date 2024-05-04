using TangoSchool.ApplicationServices.Models.LessonRequests;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface ILessonRequestsService
{
    Task<Guid> CreateLessonRequest
    (
        Guid userId,
        CreateLessonRequestPayload payload,
        CancellationToken cancellationToken
    );
    
    Task<GetLessonRequestByTeacherResponse> GetLessonRequestsByTeacher
    (
        Guid teacherId,
        GetLessonRequestByTeacherPayload payload,
        CancellationToken cancellationToken
    );
    
    Task<GetLessonRequestByTeacherResponse> GetLessonRequestsByStudent
    (
        Guid userId,
        GetLessonRequestByTeacherPayload payload,
        CancellationToken cancellationToken
    );

    Task RejectLessonRequest
    (
        Guid userId,
        Guid id,
        CancellationToken cancellationToken
    );

    Task ApproveLessonRequest
    (
        Guid userId,
        Guid lessonRequestId,
        ApproveLessonRequestPayload payload,
        CancellationToken cancellationToken
    );
}
