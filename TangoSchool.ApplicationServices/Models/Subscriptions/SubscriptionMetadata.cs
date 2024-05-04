using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.SubscriptionTemplates;

namespace TangoSchool.ApplicationServices.Models.Subscriptions;

public record SubscriptionMetadata
(
    List<StudentHeader> Students,
    List<SubscriptionTemplateHeader> SubscriptionTemplates
);
