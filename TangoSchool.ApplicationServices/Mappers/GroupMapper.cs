using TangoSchool.ApplicationServices.Models.Groups;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class GroupMapper
{
    public static Group MapToDatabaseGroup(this CreateGroupPayload model)
    {
        return new()
        {
            Name = model.Name,
            Description = model.Description,
            Level = model.Level,
            MaxStudentCapacity = model.MaxStudentCapacity,
            TeacherId = model.TeacherId,
            JoinedStudentGroups = model.studentIds
                .Select(x => new StudentGroup()
                {
                    StudentId = x,
                    JoinDate = DateTimeOffset.UtcNow
                })
                .ToList()
        };
    }
}
