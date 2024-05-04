namespace TangoSchool.ApplicationServices.Models.Classrooms;

public record GetAvailableClassroomsPayload
(
    DateTimeOffset StartTime,
    DateTimeOffset FinishTime
);
