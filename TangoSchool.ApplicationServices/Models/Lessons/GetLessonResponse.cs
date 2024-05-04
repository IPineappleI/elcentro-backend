using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.ApplicationServices.Models.Groups;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Lessons;

public record GetLessonResponse
(
    Guid Id,
    string Name,
    string? Description,
    LessonType LessonType,
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime,
    ClassroomHeader Classroom,
    TeacherHeader Teacher,
    StudentHeader? Student,
    GroupHeader? Group
);
