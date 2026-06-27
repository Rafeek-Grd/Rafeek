using Microsoft.Extensions.Logging;
using Rafeek.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace Rafeek.Infrastructure.Notifications.Sms
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;

        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public Task SendSmsAsync(string phoneNumber, string message)
        {
            _logger.LogInformation($"[MOCK SMS] Sending SMS to {phoneNumber}: {message}");
            return Task.CompletedTask;
        }
    }
}
