using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class ClassroomMapper
{
    public static Classroom MapToDatabaseClassroom(this CreateClassroomPayload model)
    {
        return new()
        {
            Name = model.Name,
            Description = model.Description
        };
    }
}
