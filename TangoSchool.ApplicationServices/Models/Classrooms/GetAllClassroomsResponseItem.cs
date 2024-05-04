namespace TangoSchool.ApplicationServices.Models.Classrooms;

public record GetAllClassroomsResponseItem
(
    Guid Id,
    string Name,
    string? Description,
    bool Terminated
);
