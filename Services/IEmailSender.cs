using System.Threading.Tasks;

namespace ZRdotnetcore.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
