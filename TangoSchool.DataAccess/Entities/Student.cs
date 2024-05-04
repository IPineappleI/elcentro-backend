using TangoSchool.DataAccess.Entities.Interfaces;
using TangoSchool.DataAccess.Enums;

namespace TangoSchool.DataAccess.Entities;

public class Student : IPersistent
{
    public Guid Id { get; set; }

    public bool Terminated { get; set; }

    public Guid ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;

    public DanceProficiencyLevel Level { get; set; }

    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public ICollection<LessonRequest> LessonRequests { get; set; } = new List<LessonRequest>();
    
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
