using System.ComponentModel.DataAnnotations;

namespace TangoSchool.ApplicationServices.Models.Identities;

public class UpdateUserRequest
{
    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Отчество")]
    public string? MiddleName { get; set; }

    [Required]
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Фото")]
    public string? Photo { get; set; }
}
