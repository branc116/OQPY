using System.Threading.Tasks;

namespace OQPYManager.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}