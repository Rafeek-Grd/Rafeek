using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Options;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Rafeek.Infrastructure.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly AiIntegrationSettings _settings;
        private readonly ILogger<AIService> _logger;

        private static readonly JsonSerializerOptions _snakeCaseOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private static readonly JsonSerializerOptions _deserializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AIService(HttpClient httpClient, IOptions<AiIntegrationSettings> settings, ILogger<AIService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<AIRecommendationDto> GetRecommendationsAsync(StudentAIGradesDto studentData, CancellationToken cancellationToken)
        {
            var path = _settings.Services.AiRecommendation.Replace("{student_id}", studentData.StudentId.ToString());
            var url = $"{_settings.BaseUrl}{path}";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            return await ProcessResponseAsync<AIRecommendationDto>(response, cancellationToken);
        }

        public async Task<AITimetableResponseDto> GenerateScheduleAsync(AITimetableRequestDto request, CancellationToken cancellationToken)
        {
            var url = $"{_settings.TimeTableBaseUrl}{_settings.Services.GenerateTimeTable}";

            // Load System Prompt
            var systemPrompt = await LoadPromptAsync("SchedulePrompt", cancellationToken);

            // Flatten the payload
            var payload = new
            {
                system_prompt = systemPrompt,
                option = request.Option,
                preferences = request.Preferences,
                courses = request.Courses
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload, _snakeCaseOptions, cancellationToken);
            return await ProcessResponseAsync<AITimetableResponseDto>(response, cancellationToken);
        }

        private async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
            {
                await HandleNonSuccessResponseAsync(response, ct);
            }

            var content = await response.Content.ReadAsStringAsync(ct);
            var cleaned = CleanAiResponse(content);

            if (TryExtractError(cleaned, out var errorMessage))
            {
                throw new BadRequestException(errorMessage!);
            }

            try
            {
                var result = JsonSerializer.Deserialize<T>(cleaned, _deserializerOptions);
                return result ?? throw new BadRequestException("Failed to deserialize AI response.");
            }
            catch (JsonException)
            {
                throw new BadRequestException("AI returned an invalid response format.");
            }
        }

        private async Task<string> LoadPromptAsync(string promptName, CancellationToken ct)
        {
            var promptPath = Path.Combine(AppContext.BaseDirectory, "AI", "Prompts", $"{promptName}.txt");
            return await File.ReadAllTextAsync(promptPath, ct);
        }

        private async Task HandleNonSuccessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (TryExtractError(errorContent, out var errorMessage))
            {
                throw new BadRequestException(errorMessage!);
            }

            response.EnsureSuccessStatusCode();
        }

        private bool TryExtractError(string content, out string? errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrWhiteSpace(content)) return false;

            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    errorMessage = errorProp.GetString();
                    return !string.IsNullOrEmpty(errorMessage);
                }
            }
            catch (JsonException) { }

            return false;
        }

        private string CleanAiResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            // Remove markdown code blocks if present
            var match = Regex.Match(input, @"```(?:json)?\s*([\s\S]*?)\s*```");
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return input.Trim();
        }
    }
}
