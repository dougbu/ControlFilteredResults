using System.Threading.Tasks;

namespace ControlFilteredResults.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
