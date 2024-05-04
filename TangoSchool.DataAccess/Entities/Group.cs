using TangoSchool.DataAccess.Entities.Interfaces;
using TangoSchool.DataAccess.Enums;

namespace TangoSchool.DataAccess.Entities;

public class Group : IPersistent
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DanceProficiencyLevel Level { get; set; }

    public int MaxStudentCapacity { get; set; }

    public Guid TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;

    public bool Terminated { get; set; }

    public ICollection<StudentGroup> JoinedStudentGroups { get; set; } = new List<StudentGroup>();

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
