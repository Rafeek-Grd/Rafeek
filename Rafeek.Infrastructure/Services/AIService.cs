using Microsoft.Extensions.Options;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models.AI;
using Rafeek.Application.Common.Options;
using System.Net.Http.Json;

namespace Rafeek.Infrastructure.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly AiIntegrationSettings _settings;

        public AIService(HttpClient httpClient, IOptions<AiIntegrationSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<AIRecommendationDto> GetRecommendationsAsync(StudentAIGradesDto studentData, CancellationToken cancellationToken)
        {
            var path = _settings.Services.AiRecommendation.Replace("{student_id}", studentData.StudentId.ToString());
            var url = $"{_settings.BaseUrl}{path}";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AIRecommendationDto>(cancellationToken: cancellationToken);
            return result ?? throw new BadRequestException("Failed to deserialize AI recommendation response.");
        }
    }
}
