using TangoSchool.ApplicationServices.Models.SubscriptionTemplates;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class SubscriptionTemplateMapper
{
    public static SubscriptionTemplate MapToDatabaseSubscriptionTemplate(this CreateSubscriptionTemplatePayload model)
    {
        return new()
        {
            Name = model.Name,
            Description = model.Description,
            LessonType = model.LessonType,
            LessonCount = model.LessonCount,
            ExpirationDate = model.ExpirationDate,
            ExpirationDayCount = model.ExpirationDayCount,
            Price = model.Price,
            Active = model.Active
        };
    }
}
