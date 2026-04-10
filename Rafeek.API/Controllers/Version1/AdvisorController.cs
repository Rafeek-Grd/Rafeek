using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.AdvisorHandlers.Commands;
using Rafeek.Application.Handlers.AdvisorHandlers.Queries;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class AdvisorController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdvisorController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Advisor.GetPendingGuidanceRequests)]
        public async Task<IActionResult> GetPendingGuidanceRequests([FromRoute] Guid advisorId)
        {
            var query = new GetPendingGuidanceRequestsQuery { AdvisorId = advisorId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch]
        [Route(ApiRoutes.Advisor.ReviewGuidanceRequest)]
        public async Task<IActionResult> ReviewGuidanceRequest([FromRoute] Guid advisorId, [FromRoute] Guid requestId, [FromBody] ReviewStudentGuidanceRequestCommand command)
        {
            try
            {
                command.AdvisorId = advisorId;
                command.RequestId = requestId;
                
                var result = await _mediator.Send(command);
                return Ok(result, "Guidance request reviewed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
