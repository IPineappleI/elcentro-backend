using TangoSchool.DataAccess.Entities.Interfaces;

namespace TangoSchool.DataAccess.Entities;

public class Teacher : IPersistent
{
    public Guid Id { get; set; }

    public bool Terminated { get; set; }

    public Guid ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public ICollection<LessonRequest> LessonRequests { get; set; } = new List<LessonRequest>();

    public ICollection<Group> Groups { get; set; } = new List<Group>();
}
