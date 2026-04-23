using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AIHandlers.Commands.SaveAITimetable;
using Rafeek.Application.Handlers.AIHandlers.Queries.GetAICourseRecommendations;
using Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetChatHistory;
using Rafeek.Application.Handlers.CareerHandlers.Queries.GetCareerSuggestionsByStudent;
using Rafeek.Application.Handlers.StudyPlanHandlers.Queries.GetStudyPlanByStudent;
using Rafeek.Application.Handlers.LearningResourceHandlers.Queries.GetAllLearningResources;
using Rafeek.Application.Handlers.AIHandlers.DTOs;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetAiSessions;
using Rafeek.Application.Handlers.StudentHandlers.Commands.AskAi;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AiController: BaseApiController
    {
        public AiController(IMediator mediator, IStringLocalizer<Messages> localizer): base(mediator, localizer)
        {
        }


        /// <summary>
        /// Get AI-driven course recommendations for a student.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.AiIntegration.GetRecommendations)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecommendations(Guid studentId)
        {
            var result = await _mediator.Send(new GetAICourseRecommendationsQuery { StudentId = studentId });
            return Ok(result);
        }

        /// <summary>
        /// Generate an AI-optimized timetable based on student preferences and course offerings.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.AiIntegration.GenerateTimetable)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateTimetable([FromBody] AITimetableRequestDto request)
        {
            var result = await _mediator.Send(new GenerateAITimetableCommand { TimetableRequest = request });
            return Ok(result);
        }

        /// <summary>
        /// Save a generated AI timetable for a student.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.AiIntegration.SaveTimetable)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveTimetable([FromBody] SaveAITimetableCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Ask the AI Chatbot a question.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.AiIntegration.AskAi)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AskAiChat([FromBody] AskAiCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all AI Chatbot sessions for the currently logged-in student.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.AiIntegration.GetAiSessions)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAiSessions()
        {
            var result = await _mediator.Send(new GetAiSessionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get Chat History for a specific AI Chatbot session, with pagination support.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.AiIntegration.GetChatHistory)]
        [ProducesResponseType(typeof(List<ChatHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChatHistory([FromQuery] GetChatHistoryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get AI-driven career suggestions for a student.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.CareerSuggestion.GetByStudent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCareerSuggestionsByStudent(Guid studentId)
        {
            var result = await _mediator.Send(new GetCareerSuggestionsByStudentQuery { StudentId = studentId });
            return Ok(result);
        }

        /// <summary>
        /// Get Study Plan for a student.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.StudyPlan.GetByStudent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudyPlansByStudent(Guid studentId)
        {
            var result = await _mediator.Send(new GetStudyPlanByStudentQueryPagginated { StudentId = studentId });
            return Ok(result);
        }

        /// <summary>
        /// Get All Learning Resources/
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.LearningResource.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllLearningResources([FromQuery] ResourceType? resourceType)
        {
            var result = await _mediator.Send(new GetAllLearningResourcesQueryPagginated { ResourceType = resourceType });
            return Ok(result);
        }
    }
}
