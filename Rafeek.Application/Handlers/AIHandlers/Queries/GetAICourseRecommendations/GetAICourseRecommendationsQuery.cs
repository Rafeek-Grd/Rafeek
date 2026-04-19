using MediatR;
using Rafeek.Application.Common.Models.AI;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAICourseRecommendations
{
    public class GetAICourseRecommendationsQuery : IRequest<AIRecommendationDto>
    {
        [JsonPropertyName("student_id")]
        public Guid StudentId { get; set; }
    }
}
