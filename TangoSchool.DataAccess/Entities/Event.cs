using TangoSchool.DataAccess.Enums;

namespace TangoSchool.DataAccess.Entities;

public class Event
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public string? Picture { get; set; }

    public EventType EventType { get; set; }
}
