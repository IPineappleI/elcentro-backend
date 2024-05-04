namespace TangoSchool.ApplicationServices.Models.LessonRequests;

public record ApproveLessonRequestPayload
(
    string Name,
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    Guid ClassroomId
);
