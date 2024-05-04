using TangoSchool.ApplicationServices.Models.Emails;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmail
    (
        SendEmailPayload payload,
        CancellationToken cancellationToken
    );
}
