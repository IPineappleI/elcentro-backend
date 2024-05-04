using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Groups;

public record GroupHeader(Guid Id, string Name, int StudentCount, DanceProficiencyLevel Level);
