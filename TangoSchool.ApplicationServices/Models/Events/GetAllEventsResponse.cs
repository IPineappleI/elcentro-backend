namespace TangoSchool.ApplicationServices.Models.Events;

public record GetAllEventsResponse
(
    List<GetAllEventsResponseItem> Items,
    int TotalCount
);
