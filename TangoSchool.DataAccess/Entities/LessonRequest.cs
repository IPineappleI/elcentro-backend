using TangoSchool.DataAccess.Entities.Interfaces;

namespace TangoSchool.DataAccess.Entities;

public class LessonRequest : IPersistent
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset FinishTime { get; set; }

    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public Guid TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;

    public bool Terminated { get; set; }
}
