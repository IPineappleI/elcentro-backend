namespace TangoSchool.ApplicationServices.Models.Students;

public record StudentHeader
(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName
);
