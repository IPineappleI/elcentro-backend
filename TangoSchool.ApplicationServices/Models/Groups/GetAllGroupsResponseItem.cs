using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Groups;

public record GetAllGroupsResponseItem
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
