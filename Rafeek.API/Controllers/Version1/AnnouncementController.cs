using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.API.Filters;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.AnnouncementHandlers.Commands.CreateAnnouncement;
using Rafeek.Application.Handlers.AnnouncementHandlers.Commands.DeactivateAnnouncement;
using Rafeek.Application.Handlers.AnnouncementHandlers.Commands.PostponeAnnouncement;
using Rafeek.Application.Handlers.AnnouncementHandlers.Queries.GetAnnouncementsPaginated;
using System;
using System.Threading.Tasks;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AnnouncementController : BaseApiController
    {
        public AnnouncementController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Creates a new broadcast announcement (Admin and Staff only).
        /// </summary>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.Announcement.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves paginated list of announcements (Admin and Staff only).
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.Announcement.GetAllPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagginated([FromQuery] GetAnnouncementsPaginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Deactivates an announcement (Admin and Staff only).
        /// </summary>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.Announcement.Deactivate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _mediator.Send(new DeactivateAnnouncementCommand { Id = id });
            if (!result) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Reschedules/Postpones an announcement (Admin and Staff only).
        /// </summary>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.Announcement.Postpone)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Postpone(Guid id, [FromBody] PostponeAnnouncementCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return Ok(result);
        }
    }
}
