using TangoSchool.DataAccess.Enums;

namespace TangoSchool.DataAccess.Entities;

public class SubscriptionTemplate
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public LessonType LessonType { get; set; }

    public int LessonCount { get; set; }

    public DateTimeOffset? ExpirationDate { get; set; }

    public int? ExpirationDayCount { get; set; }

    public decimal Price { get; set; }

    public bool Active { get; set; }
}
