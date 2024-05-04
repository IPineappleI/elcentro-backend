namespace TangoSchool.ApplicationServices.Models.Lessons;

public record GetAllLessonsPayload
(
    DateTimeOffset From,
    DateTimeOffset To,
    bool IncludeTerminated
);
