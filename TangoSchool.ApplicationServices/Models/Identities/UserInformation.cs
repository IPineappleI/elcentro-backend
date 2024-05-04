namespace TangoSchool.ApplicationServices.Models.Identities;

public class UserInformation
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Photo { get; set; }
    
    public string? Role { get; set; }
}
