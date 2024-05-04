using TangoSchool.DataAccess.Entities.Interfaces;

namespace TangoSchool.DataAccess.Entities;

public class Classroom : IPersistent
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool Terminated { get; set; }

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
