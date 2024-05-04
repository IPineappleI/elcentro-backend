using TangoSchool.ApplicationServices.Models.Events;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface IEventService
{
    Task<Guid> CreateEvent
    (
        CreateEventPayload payload,
        CancellationToken cancellationToken
    );

    Task UpdateEvent
    (
        UpdateEventPayload payload,
        CancellationToken cancellationToken
    );

    Task<GetEventResponse> GetEvent
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task<GetAllEventsResponse> GetAllEvents
    (
        GetAllEventsPayload payload,
        CancellationToken cancellationToken
    );
}
