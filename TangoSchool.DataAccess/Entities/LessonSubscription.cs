namespace TangoSchool.DataAccess.Entities;

public class LessonSubscription
{
    public Guid Id { get; set; }

    public Guid LessonId { get; set; }

    public Lesson Lesson { get; set; } = null!;

    public Guid SubscriptionId { get; set; }

    public Subscription Subscription { get; set; } = null!;
}
