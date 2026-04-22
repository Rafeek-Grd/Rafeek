using Rafeek.Application.Handlers.ExternalHandlers.DTOs;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IAIService
    {
        Task<AIRecommendationDto> GetRecommendationsAsync(StudentAIGradesDto studentData, CancellationToken cancellationToken);
        Task<AITimetableResponseDto> GenerateScheduleAsync(AITimetableRequestDto request, CancellationToken cancellationToken);
    }
}
