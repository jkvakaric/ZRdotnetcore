using System.Threading.Tasks;

namespace ZRdotnetcore.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
