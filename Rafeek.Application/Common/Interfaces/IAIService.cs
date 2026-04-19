using Rafeek.Application.Common.Models.AI;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IAIService
    {
        Task<AIRecommendationDto> GetRecommendationsAsync(StudentAIGradesDto studentData, CancellationToken cancellationToken);
    }
}
