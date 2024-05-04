using TangoSchool.DataAccess.Enums;

namespace TangoSchool.DataAccess.Entities;

public class Subscription
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public LessonType LessonType { get; set; }

    public int LessonCount { get; set; }

    public DateTimeOffset ExpirationDate { get; set; }

    public decimal Price { get; set; }

    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public Guid? SubscriptionTemplateId { get; set; }

    public SubscriptionTemplate? SubscriptionTemplate { get; set; }

    public ICollection<LessonSubscription> AttendedLessons { get; set; } = new List<LessonSubscription>();
}
