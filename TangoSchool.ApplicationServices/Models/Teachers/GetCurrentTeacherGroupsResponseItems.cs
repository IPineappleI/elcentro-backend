using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Teachers;

public record GetCurrentTeacherGroupsResponseItems
(
    Guid Id,
    string Name,
    string? Description,
    DanceProficiencyLevel Level,
    int StudentCapacity,
    int MaxStudentCapacity,
    TeacherHeader Teacher,
    bool Terminated
);
