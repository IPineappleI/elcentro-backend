namespace TangoSchool.ApplicationServices.Models.Lessons;

public record SetLessonAttendancePayload
(
    List<Guid> StudentIds
);
