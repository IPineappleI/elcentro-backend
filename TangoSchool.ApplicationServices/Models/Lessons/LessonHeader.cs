using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Lessons;

public record LessonHeader
(
    Guid Id,
    string Name,
    LessonType LessonType,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime
);
