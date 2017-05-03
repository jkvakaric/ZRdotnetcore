using System.Threading.Tasks;
using Microsoft.Extensions.Options;

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

        public Task SendEmailAsync(string email, string subject, string message)
        {
            //var mailMessage = new SendGrid.SendGridMessage();
            //mailMessage.AddTo(email);
            //mailMessage.From = new MailAddress("postmaster@zr.net", "ZRdotnetcore");
            //mailMessage.Subject = subject;
            //mailMessage.Text = message;
            //mailMessage.Html = message;
            //var credentials = new NetworkCredential(
            //    Options.SendGridUser,
            //    Options.SendGridKey);
            //var transport = new SendGrid.Web(credentials);
            //return transport.DeliverAsync(mailMessage);

            // temporary sendgrid fix - using mailgun
            //var client = new HttpClient { BaseAddress = new Uri(Options.MailgunApiUri) };
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            //    Convert.ToBase64String(Encoding.UTF8.GetBytes(Options.MailgunApiKey)));
            //var form = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("from", "postmaster@PicPository.net"),
            //    new KeyValuePair<string, string>("to", email),
            //    new KeyValuePair<string, string>("subject", subject),
            //    new KeyValuePair<string, string>("html", message)
            //});
            //var response = client.PostAsync(Options.MailgunApiRoute, form);
            //return response;
            return Task.FromResult(0);
        }
    }
}
