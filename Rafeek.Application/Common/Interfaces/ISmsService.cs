using System.Threading.Tasks;

namespace Rafeek.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phoneNumber, string message);
    }
}
