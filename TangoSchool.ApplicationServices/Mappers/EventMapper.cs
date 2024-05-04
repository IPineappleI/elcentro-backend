using TangoSchool.ApplicationServices.Models.Events;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.ApplicationServices.Mappers;

internal static class EventMapper
{
    public static Event MapToDatabaseEvent(this CreateEventPayload model)
    {
        return new()
        {
            Name = model.Name,
            Description = model.Description,
            StartTime = model.StartTime,
            Picture = model.Picture,
            EventType = model.EventType
        };
    }
}
