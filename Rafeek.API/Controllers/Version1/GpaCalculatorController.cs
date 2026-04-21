using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.GPAHandlers.Commands.SimulateGPA;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class GpaCalculatorController : BaseApiController
    {
        public GpaCalculatorController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Simulate GPA for a student based on expected GPA and current courses.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.GpaCalculator.Simulate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Simulate([FromBody] SimulateGPACommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
