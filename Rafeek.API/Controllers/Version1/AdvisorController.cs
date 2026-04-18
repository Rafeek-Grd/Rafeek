using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AdvisorHandlers.Commands.AssignStudentsToAcademicAdvisor;
using Rafeek.Application.Handlers.AdvisorHandlers.Commands.UpdateStatusOfGudienceRequest;
using Rafeek.Application.Handlers.AdvisorHandlers.Queries.GetAllGuidenceSupportRequests;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AdvisorController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdvisorController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Assign a list of students to an academic advisor.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("Advisor")]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Student.AssignStudentsToAcademicAdvisor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignToAcademicAdvisor([FromBody] AssignStudentsToAcademicAdvisorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all guidance requests with pagination, filtering, and searching capabilities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.Advisor.GetAllGuidanceRequestsPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllGuidanceRequestsPagginated([FromQuery] GetAllGuidenceSupportRequestsPagginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Update status of a guidance request (approve, reject, or request more information).
        /// </summary>
        /// <param name="requestId">The ID of the guidance request to update.</param>
        /// <param name="command">The command containing the new status and any additional information.</param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Doctor), nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Advisor.UpdateGuidanceRequestStatus)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGuidenceRequestStatus(Guid requestId, [FromBody] UpdateStatusOfGuidenceRequestCommand command)
        {
            command.RequestId = requestId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
