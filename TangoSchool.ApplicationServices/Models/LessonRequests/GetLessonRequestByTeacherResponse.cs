namespace TangoSchool.ApplicationServices.Models.LessonRequests;

public record GetLessonRequestByTeacherResponse
(
    List<GetLessonRequestByTeacherResponseItem> Items,
    int TotalCount
);
