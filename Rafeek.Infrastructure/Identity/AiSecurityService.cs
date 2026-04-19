using Microsoft.Extensions.Options;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Options;

namespace Rafeek.Infrastructure.Identity
{
    public class AiSecurityService : IAiSecurityService
    {
        private readonly AiEngineSettings _aiEngineSettings;

        public AiSecurityService(IOptions<AiEngineSettings> aiEngineSettings)
        {
            _aiEngineSettings = aiEngineSettings.Value;
        }

        public bool ValidateApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            return apiKey.Equals(_aiEngineSettings.ApiKey);
        }
    }
}
