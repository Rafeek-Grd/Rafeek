using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Extensions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.AddEventToAcademicCalendar;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.DeleteEventOfAcademicCalendar;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.UpdateEventOfAcademicCalendar;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Query.GetAllPagginatedEvents;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Query.GetEventOfAcademicCalendarById;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AcademicCalendarController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        public AcademicCalendarController(IMediator mediator, IStringLocalizer<Messages> localizer, ICurrentUserService currentUserService) : base(mediator, localizer, currentUserService)
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
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Doctor), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.EventOfAcademicCalendar.AddEvent)]
        [ProducesResponseType(typeof(AcademicCalendarDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddEventToAcademicCalendar([FromBody] AddEventToAcademicCalendarCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(null!, result);
        }


        /// <summary>
        /// Update an existing event in the academic calendar.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Doctor), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.EventOfAcademicCalendar.UpdateEvent)]
        [ProducesResponseType(typeof(AcademicCalendarDto), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventOfAcademicCalendar([FromBody] UpdateEventOfAcademicCalendarCommand command)
        {
            var result = await _mediator.Send(command);
            return Accepted(result);
        }


        /// <summary>
        /// Delete Specific event from the academic calendar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.EventOfAcademicCalendar.DeleteEvent)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventFromAcademicCalendar(string id)
        {
            var result = await _mediator.Send(new DeleteEventOfAcademicCalendarCommand() { AcademicEventId = id});
            return Deleted(result);
        }


        /// <summary>
        /// Get All Events of the academic calendar in a paginated format.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.EventOfAcademicCalendar.GetAllPagginatedEvents)]
        [ProducesResponseType(typeof(PagginatedResult<AcademicCalendarDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPagginatedOfAcademicCalendar([FromQuery]GetAllEventsPagginatedQuery filter)
        {
            var result = await _mediator.Send(filter);
            return Ok(result);
        }

        /// <summary>
        /// Get specific event of the academic calendar by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.EventOfAcademicCalendar.GetEventById)]
        [ProducesResponseType(typeof(AcademicCalendarDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventOfAcademicCalendarById(string id)
        {
            var result = await _mediator.Send(new GetEventOfAcademicCalendarByIdQuery { AcademicCalendarId = id.ToGuid() });
            return Ok(result);
        }

    }
}
