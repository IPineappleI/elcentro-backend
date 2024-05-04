namespace TangoSchool.ApplicationServices.Models.Classrooms;

public record GetAllClassroomsPayload
(
    int Page,
    int ItemsPerPage,
    bool IncludeTerminated
);
