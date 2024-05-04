using TangoSchool.ApplicationServices.Models.LessonRequests;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class LessonRequestMapper
{
    public static LessonRequest MapToDatabaseLesson(this CreateLessonRequestPayload model)
    {
        return new()
        {
            Description = model.Description,
            StartTime = model.StartTime,
            FinishTime = model.FinishTime,
            TeacherId = model.TeacherId
        };
    }
}
