using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.SubscriptionTemplates;

public record SubscriptionTemplateHeader
(
    Guid Id,
    string Name,
    LessonType LessonType,
    decimal Price
);
