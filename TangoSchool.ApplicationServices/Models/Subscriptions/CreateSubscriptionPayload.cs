using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Subscriptions;

public record CreateSubscriptionPayload
(
    string Name,
    string? Description,
    LessonType LessonType,
    int LessonCount,
    DateTimeOffset ExpirationDate,
    decimal Price,
    Guid StudentId,
    Guid? SubscriptionTemplateId
);
