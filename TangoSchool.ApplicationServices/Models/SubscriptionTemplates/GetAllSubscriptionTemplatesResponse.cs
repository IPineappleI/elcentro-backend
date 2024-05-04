namespace TangoSchool.ApplicationServices.Models.SubscriptionTemplates;

public record GetAllSubscriptionTemplatesResponse
(
    List<GetAllSubscriptionTemplatesResponseItem> Items,
    int TotalCount
);
