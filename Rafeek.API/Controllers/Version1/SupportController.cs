using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudentSupportHandlers.Commands.DeleteStudentSupportTicket;
using Rafeek.Application.Handlers.StudentSupportHandlers.Commands.NewStudentSupportTicket;
using Rafeek.Application.Handlers.StudentSupportHandlers.Commands.UpdateStudentSupportTicket;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;
using Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllActiveStudentSupportForCurrentUser;
using Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllSudentSupportTickets;
using Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetStudentSupportById;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class SupportController : BaseApiController
    {
        public SupportController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Creates a new student support ticket.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.StudentSupport.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] NewStudentSupportTicketCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Gets all student support tickets.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.StudentSupport.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllSudentSupportTicketsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get my active student support tickets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.StudentSupport.GetMyActive)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyActive()
        {
            var query = new GetAllActiveStudentSupportForCurrentUserQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get student support ticket by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.StudentSupport.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetStudentSupportByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Update student support ticket.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize]
        [Route(ApiRoutes.StudentSupport.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStudentSupportTicketCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete student support ticket.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.StudentSupport.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromQuery]DeleteStudentSupportTicketCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
