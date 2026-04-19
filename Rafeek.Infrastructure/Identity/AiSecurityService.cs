using Microsoft.Extensions.Configuration;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Infrastructure.Identity
{
    public class AiSecurityService : IAiSecurityService
    {
        private readonly IConfiguration _configuration;

        public AiSecurityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            var expectedKey = _configuration.GetValue<string>("AiEngineSettings:ApiKey");
            return apiKey.Equals(expectedKey);
        }
    }
}
