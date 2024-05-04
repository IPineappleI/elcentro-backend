using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Groups;

public record GetGroupResponse
(
    string Name,
    string? Description,
    DanceProficiencyLevel Level,
    int MaxStudentCapacity,
    TeacherHeader Teacher,
    List<StudentHeader> Students
);
