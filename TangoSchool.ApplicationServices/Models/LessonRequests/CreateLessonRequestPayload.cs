namespace TangoSchool.ApplicationServices.Models.LessonRequests;

public record CreateLessonRequestPayload
(
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    Guid TeacherId
);
