using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Models.AI;
using Rafeek.Application.Handlers.AIHandlers.Commands.SaveAITimetable;
using Rafeek.Application.Handlers.AIHandlers.Queries.GetAICourseRecommendations;
using Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetChatHistory;

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
        [Authorize]
        [Route(ApiRoutes.AiIntegration.AskAi)]
        [ProducesResponseType(typeof(AiChatResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AskAiChat([FromBody] Rafeek.Application.Handlers.StudentHandlers.Commands.AskAi.AskAiCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all AI Chatbot sessions for the currently logged-in student.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route(ApiRoutes.AiIntegration.GetAiSessions)]
        [ProducesResponseType(typeof(List<AiSessionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAiSessions()
        {
            var result = await _mediator.Send(new Rafeek.Application.Handlers.StudentHandlers.Query.GetAiSessions.GetAiSessionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get the chat history of the currently logged-in student with the AI Chatbot.
        /// </summary>
        /// <param name="sessionId">The ID of the session to get history for (optional)</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 20)</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route(ApiRoutes.AiIntegration.GetChatHistory)]
        [ProducesResponseType(typeof(List<ChatHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChatHistory([FromQuery] Guid? sessionId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetChatHistoryQuery { SessionId = sessionId, Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
