using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.ReminderHandlers.Commands.CreateReminder;
using Rafeek.Application.Handlers.ReminderHandlers.Commands.UpdateReminder;
using Rafeek.Application.Handlers.ReminderHandlers.Commands.DeleteReminder;
using Rafeek.Application.Handlers.ReminderHandlers.Queries.GetRemindersPaginated;
using Rafeek.Application.Localization;
using Rafeek.API.Filters;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class ReminderController : BaseApiController
    {
        public ReminderController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Creates a new reminder for the authenticated user.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.Reminder.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateReminderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Gets all reminders paginated for the authenticated user.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Reminder.GetAllPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPagginated([FromQuery] GetRemindersPaginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing reminder.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize]
        [Route(ApiRoutes.Reminder.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReminderCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            
            return Ok(result);
        }

        /// <summary>
        /// Deletes a reminder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize]
        [Route(ApiRoutes.Reminder.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteReminderCommand { Id = id });
            if (!result) return NotFound();
            
            return Ok(result);
        }
    }
}
