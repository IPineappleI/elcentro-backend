using System.ComponentModel.DataAnnotations;
using TangoSchool.DataAccess.Enums;

namespace TangoSchool.ApplicationServices.Models.Identities;

public class RegisterStudentPayload
{
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Отчество")] public string? MiddleName { get; set; }

    [Required]
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Фото")]
    public string? Photo { get; set; }

    [Display(Name = "Описание")]
    public string? Description { get; set; }
    
    [Required]
    [Display(Name = "Уровень владения танцем")]
    public DanceProficiencyLevel Level { get; set; }
}
