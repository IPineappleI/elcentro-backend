using TangoSchool.ApplicationServices.Models.SubscriptionTemplates;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface ISubscriptionTemplatesService
{
    Task<Guid> CreateSubscriptionTemplate
    (
        CreateSubscriptionTemplatePayload payload,
        CancellationToken cancellationToken
    );

    Task UpdateSubscriptionTemplate
    (
        UpdateSubscriptionTemplate payload,
        CancellationToken cancellationToken
    );

    Task<GetSubscriptionTemplateResponse> GetSubscriptionTemplate
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task<GetAllSubscriptionTemplatesResponse> GetAllSubscriptionTemplates
    (
        GetAllSubscriptionTemplatesPayload payload,
        CancellationToken cancellationToken
    );
}
