namespace TangoSchool.ApplicationServices.Models.LessonRequests;

public record GetLessonRequestByTeacherResponseItem
(
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    Guid StudentId,
    Guid TeacherId
);
