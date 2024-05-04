namespace TangoSchool.ApplicationServices.Models.Events;

public record GetAllEventsPayload
(
    int Page,
    int ItemsPerPage
);
