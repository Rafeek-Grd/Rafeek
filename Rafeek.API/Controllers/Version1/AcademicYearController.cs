using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands.AddAcademicYearCommand;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands.UpdateAcademicYear;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands.DeleteAcademicYear;
using Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAllAcademicYear;
using Rafeek.API.Filters;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAcademicYearById;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AcademicYearController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AcademicYearController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add new academic year to the system. Only Admin and SubAdmin can perform this action.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.AcademicYear.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddAcademicYearCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Update existing academic year in the system. Only Admin and SubAdmin can perform this action.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.AcademicYear.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAcademicYearCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete academic year
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.AcademicYear.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteAcademicYearCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all academic years pagginated
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.AcademicYear.GetAllPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery]GetAllAcademicYearsPagginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get academic year by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.AcademicYear.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetAcademicYearByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
