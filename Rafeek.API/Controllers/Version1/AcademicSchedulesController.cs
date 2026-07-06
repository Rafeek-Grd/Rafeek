using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AcademicSchedules.Commands.CreateAcadmicSchedule;
using Rafeek.Application.Handlers.AcademicSchedules.Commands.DeleteAcademicSchedule;
using Rafeek.Application.Handlers.AcademicSchedules.Commands.UpdateAcadmicSchedule;
using Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedules;
using Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedulesById;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AcademicSchedulesController : BaseApiController
    {
        public AcademicSchedulesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all academic schedules with optional filtering and pagination.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Mentor), nameof(UserType.Professor))]
        [Route(ApiRoutes.AcademicSchedules.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAcademicSchedules([FromQuery] GetAcademicSchedulesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific academic schedule by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Mentor), nameof(UserType.Professor))]
        [Route(ApiRoutes.AcademicSchedules.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAcademicScheduleById(Guid id)
        {
            var query = new GetAcademicSchedulesByIdQuery { LectureId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new academic schedule.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.AcademicSchedules.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateAcadmicScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(ApiRoutes.AcademicSchedules.GetById.Replace("{id}", result.LectureGroupId.ToString()), result);
        }

        /// <summary>
        /// Update an existing academic schedule by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.AcademicSchedules.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAcadmicScheduleCommand command)
        {
            command.LectureId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        /// <summary>
        /// Delete an academic schedule by its ID.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.AcademicSchedules.Delete)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromBody] DeleteAcademicScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return Deleted(result);
        }
    }
}
