using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Mappers;
using TangoSchool.ApplicationServices.Models.Events;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class EventService : IEventService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly IEventRepository _eventRepository;

    public EventService
    (
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext,
        IEventRepository eventRepository
    )
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
        _eventRepository = eventRepository;
    }

    public async Task<Guid> CreateEvent(CreateEventPayload payload, CancellationToken cancellationToken)
    {
        var newEvent = _eventRepository.Add(payload.MapToDatabaseEvent());

        await _eventRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newEvent.Id;
    }

    public async Task UpdateEvent(UpdateEventPayload payload, CancellationToken cancellationToken)
    {
        var currentEvent = await _readOnlyTangoSchoolDbContext
            .Events
            .Where(x => x.Id == payload.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (currentEvent is null)
        {
            throw new ApplicationException(GeneralErrorMessages.EventWasNotFound);
        }

        currentEvent.Name = payload.Name;
        currentEvent.Description = payload.Description;
        currentEvent.StartTime = payload.StartTime;
        currentEvent.Picture = payload.Picture;
        currentEvent.EventType = payload.EventType;

        _eventRepository.Update(currentEvent);

        await _eventRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetEventResponse> GetEvent(Guid id, CancellationToken cancellationToken)
    {
        var currentEvent = await _readOnlyTangoSchoolDbContext
            .Events
            .Where(x => x.Id == id)
            .Select(x => new GetEventResponse(
                x.Name,
                x.Description,
                x.StartTime,
                x.Picture,
                x.EventType
            ))
            .SingleOrDefaultAsync(cancellationToken);

        if (currentEvent is null)
        {
            throw new ApplicationException(GeneralErrorMessages.EventWasNotFound);
        }

        return currentEvent;
    }

    public async Task<GetAllEventsResponse> GetAllEvents
    (
        GetAllEventsPayload payload,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Event> query = _readOnlyTangoSchoolDbContext.Events;

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new GetAllEventsResponseItem
            (
                x.Id,
                x.Name,
                x.Description,
                x.StartTime,
                x.Picture,
                x.EventType
            ))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }
}
