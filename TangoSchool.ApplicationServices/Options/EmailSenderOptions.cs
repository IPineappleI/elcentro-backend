namespace TangoSchool.ApplicationServices.Options;

public record EmailSenderOptions
{
    public string From { get; init; } = string.Empty;

    public string Host { get; init; } = string.Empty;

    public int Port { get; init; }

    public string Password { get; init; } = string.Empty;
}
