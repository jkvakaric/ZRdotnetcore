using System.Threading.Tasks;
using SendGrid;

namespace ZRdotnetcore.Services
{
    public interface IEmailSender
    {
        Task<Response> SendEmailAsync(string email, string subject, string message);
    }
}
