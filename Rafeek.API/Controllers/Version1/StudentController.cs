using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.API.Filters;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.StudentHandlers.Commands.SendRequestForAdvismentGuide;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentDashboard;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentProfile;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentDashboard;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetChatHistory;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class StudentController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Send request for guidance to an advisor, including the title and description of the request.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.Student.SendRequestToGuide)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendRequestForGuidance([FromBody] SendRequestForAdvismentGuideCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(ApiRoutes.Advisor.GetAllGuidanceRequestsPagginated, result);
        }


        /// <summary>
        /// Get the profile of the currently logged-in student, including academic history.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.Student.GetProfile)]
        [ProducesResponseType(typeof(StudentProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetStudentProfileQuery());
            return Ok(result);
        }


        /// <summary>
        /// Get Student Dashboard data.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route(ApiRoutes.Student.GetDashboard)]
        [ProducesResponseType(typeof(StudentDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentDashboard([FromRoute] Guid userId)
        {
            var query = new GetStudentDashboardQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Ask the AI Chatbot a question.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route(ApiRoutes.Student.AskAi)]
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
        [Route(ApiRoutes.Student.GetAiSessions)]
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
        [Route(ApiRoutes.Student.GetChatHistory)]
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

