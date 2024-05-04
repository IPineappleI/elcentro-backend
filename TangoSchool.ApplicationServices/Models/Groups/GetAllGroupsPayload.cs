namespace TangoSchool.ApplicationServices.Models.Groups;

public record GetAllGroupsPayload
(
    int Page,
    int ItemsPerPage,
    bool IncludeTerminated
);
