namespace TangoSchool.ApplicationServices.Models.SubscriptionTemplates;

public record GetAllSubscriptionTemplatesPayload
(
    int Page,
    int ItemsPerPage,
    bool Active
);
