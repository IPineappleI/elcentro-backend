namespace TangoSchool.ApplicationServices.Options;

public class JwtOptions
{
    public int Expire { get; set; }
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int TokenValidityInMinutes { get; set; }
}
