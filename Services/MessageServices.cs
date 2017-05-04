using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ZRdotnetcore.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> options)
        {
            Options = options.Value;
        }

        public async Task<Response> SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var mail = new SendGridMessage
            {
                From = new EmailAddress("postmaster@zrdnc.net", "ZRdotnetcore"),
                Subject = subject,
                HtmlContent = message
            };
            mail.AddTo(new EmailAddress(email));
            Response response = await client.SendEmailAsync(mail);
            return response;
        }
    }
}
