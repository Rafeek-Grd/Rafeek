using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.AcademicTermHandlers.Queries;
using Rafeek.Application.Handlers.AcademicTermHandlers.Queries.GetAcademicTermById;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands.CreateAcademicTerm;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands.UpdateAcademicTerm;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands.DeleteAcademicTerm;
using Rafeek.API.Filters;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.AcademicTermHandlers.Queries.GetAllPagginatedAcademicTerm;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AcademicTermController : BaseApiController
    {
        public AcademicTermController(IMediator mediator, IStringLocalizer<Messages> localizer) 
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Create a new academic term.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.AcademicTerm.Create)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAcademicTermCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(ApiRoutes.AcademicTerm.GetById.Replace("{id}", result.ToString()), result);
        }


        /// <summary>
        /// Update Academic Term details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.AcademicTerm.Update)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAcademicTermCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Accepted(result);
        }


        /// <summary>
        /// Delete Academic Term
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.AcademicTerm.Delete)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteAcademicTermCommand { Id = id };
            var result = await _mediator.Send(command);
            return Deleted(result);
        }


        /// <summary>
        /// Get All Academic Terms Paginated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.AcademicTerm.GetAllPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAcademicTermPagginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get Academic Term By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.AcademicTerm.GetById)]
        [ProducesResponseType(typeof(AcademicTermDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetAcademicTermByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
