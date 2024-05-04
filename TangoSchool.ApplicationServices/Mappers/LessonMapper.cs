using TangoSchool.ApplicationServices.Models.Lessons;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class LessonMapper
{
    public static Lesson MapToDatabaseLesson(this CreateLessonPayload model)
    {
        return new()
        {
            Name = model.Name,
            Description = model.Description,
            LessonType = model.LessonType,
            StartTime = model.StartTime,
            FinishTime = model.FinishTime,
            ClassroomId = model.ClassroomId,
            TeacherId = model.TeacherId,
            StudentId = model.StudentId,
            GroupId = model.GroupId,
        };
    }
}
