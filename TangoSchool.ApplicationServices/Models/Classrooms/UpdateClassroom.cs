namespace TangoSchool.ApplicationServices.Models.Classrooms;

public record UpdateClassroom
(
    Guid Id,
    string Name,
    string? Description
);
