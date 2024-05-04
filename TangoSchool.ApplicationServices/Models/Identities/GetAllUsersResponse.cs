namespace TangoSchool.ApplicationServices.Models.Identities;

public class GetAllUsersResponse
{
    public List<UserInformation> Users { get; set; } = new();

    public int TotalCount { get; set; }
}
