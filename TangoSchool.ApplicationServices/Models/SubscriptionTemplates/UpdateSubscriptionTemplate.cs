using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.SubscriptionTemplates;

public record UpdateSubscriptionTemplate
(
    Guid Id,
    string Name,
    string? Description,
    LessonType LessonType,
    int LessonCount,
    DateTimeOffset? ExpirationDate,
    int? ExpirationDayCount,
    decimal Price,
    bool Active
);
