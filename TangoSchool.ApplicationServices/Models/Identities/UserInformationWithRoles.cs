namespace TangoSchool.ApplicationServices.Models.Identities;

public class UserInformationWithRoles : UserInformation
{
    public IList<string> Roles { get; set; } = new List<string>();
}
