namespace TangoSchool.ApplicationServices.Models.Classrooms;

public record GetClassroomResponse
(
    string Name,
    string? Description,
    bool Terminated
);
