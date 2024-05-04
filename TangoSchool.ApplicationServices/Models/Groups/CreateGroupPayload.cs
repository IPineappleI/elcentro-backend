using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Groups;

public record CreateGroupPayload
(
    string Name,
    string? Description,
    DanceProficiencyLevel Level,
    int MaxStudentCapacity,
    Guid TeacherId,
    List<Guid> studentIds
);
