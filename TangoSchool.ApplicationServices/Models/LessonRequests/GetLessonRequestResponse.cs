namespace TangoSchool.ApplicationServices.Models.LessonRequests;

public record GetLessonRequestResponse
(
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    Guid StudentId,
    Guid TeacherId
);
