using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AIHandlers.Queries.GetStudentGrades;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAICourseRecommendations
{
    public class GetAICourseRecommendationsQueryHandler : IRequestHandler<GetAICourseRecommendationsQuery, AIRecommendationDto>
    {
        private readonly IMediator _mediator;
        private readonly IAIService _aiService;

        public GetAICourseRecommendationsQueryHandler(IMediator mediator, IAIService aiService)
        {
            _mediator = mediator;
            _aiService = aiService;
        }

        public async Task<AIRecommendationDto> Handle(GetAICourseRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var studentAIGrades = await _mediator.Send(new GetStudentGradesQuery 
            { 
                StudentId = request.StudentId 
            }, cancellationToken);

            if (studentAIGrades == null || studentAIGrades.CourseGrades.Count == 0)
            {
                return new AIRecommendationDto { Status = "no_data" };
            }

            var recommendations = await _aiService.GetRecommendationsAsync(studentAIGrades, cancellationToken);

            return recommendations;
        }
    }
}
