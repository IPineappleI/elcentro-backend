namespace TangoSchool.ApplicationServices.Models.Teachers;

public record TeacherHeader
(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName
);
