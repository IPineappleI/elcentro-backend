using System.ComponentModel.DataAnnotations;

namespace TangoSchool.ApplicationServices.Models.Identities;

public class UpdateUserRolesRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Роли")]
    public HashSet<string> Roles { get; set; } = new();
}
