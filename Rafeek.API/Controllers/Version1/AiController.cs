using MediatR;
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
    }
}
