using System.ComponentModel.DataAnnotations;

namespace TangoSchool.ApplicationServices.Models.Identities;

public class ChangePasswordRequest
{
    [Required]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    public string NewPassword { get; set; } = null!;
}
