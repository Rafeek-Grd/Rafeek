using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AcademicCalendarController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        public AcademicCalendarController(IMediator mediator, IStringLocalizer<Messages> localizer, ICurrentUserService currentUserService) : base(mediator, localizer,currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Add a new event to the academic calendar.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.AcademicCalendar.AddEvent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddEventToAcademicCalendar([FromBody] AddEventToAcademicCalendarCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
