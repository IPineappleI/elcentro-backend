namespace TangoSchool.ApplicationServices.Models.Teachers;

public record GetCurrentTeacherGroupsPayload
(
    int Page,
    int ItemsPerPage,
    bool IncludeTerminated
);
