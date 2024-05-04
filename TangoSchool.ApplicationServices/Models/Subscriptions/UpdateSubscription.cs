using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Subscriptions;

public record UpdateSubscription
(
    Guid Id,
    string Name,
    string? Description,
    LessonType LessonType,
    int LessonCount,
    DateTimeOffset ExpirationDate,
    decimal Price,
    Guid StudentId
);
