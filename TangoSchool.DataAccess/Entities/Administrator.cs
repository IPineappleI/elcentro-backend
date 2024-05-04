namespace TangoSchool.DataAccess.Entities;

public class Administrator
{
    public Guid Id { get; set; }

    public Guid ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;
}
