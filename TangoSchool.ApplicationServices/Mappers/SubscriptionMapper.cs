using TangoSchool.ApplicationServices.Models.Subscriptions;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class SubscriptionMapper
{
    public static Subscription MapToDatabaseSubscription(this CreateSubscriptionPayload model)
    {
        return new()
        {
            Name = model.Name,
            Description = model.Description,
            LessonType = model.LessonType,
            LessonCount = model.LessonCount,
            ExpirationDate = model.ExpirationDate,
            Price = model.Price,
            StudentId = model.StudentId,
            SubscriptionTemplateId = model.SubscriptionTemplateId
        };
    }
}
