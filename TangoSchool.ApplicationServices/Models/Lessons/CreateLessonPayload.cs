using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Lessons;

public record CreateLessonPayload
(
    string Name,
    string? Description,
    LessonType LessonType,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    Guid ClassroomId,
    Guid TeacherId,
    Guid? StudentId,
    Guid? GroupId
);
