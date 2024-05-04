namespace TangoSchool.ApplicationServices.Models.LessonRequests;

public record UpdateLessonRequest
(
    Guid Id,
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    Guid StudentId,
    Guid TeacherId
);
