using MediatR;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAICourseRecommendations
{
    public class GetAICourseRecommendationsQuery : IRequest<AIRecommendationDto>
    {
        [JsonPropertyName("student_id")]
        public Guid StudentId { get; set; }
    }
}
