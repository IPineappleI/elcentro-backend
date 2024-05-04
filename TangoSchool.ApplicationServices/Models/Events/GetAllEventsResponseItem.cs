using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Events;

public record GetAllEventsResponseItem
(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset StartTime,
    string? Picture,
    EventType EventType
);
