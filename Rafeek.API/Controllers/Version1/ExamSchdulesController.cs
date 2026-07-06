using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.ExamSchedules.Commands.CreateExamSchdules;
using Rafeek.Application.Handlers.ExamSchedules.Commands.DeleteExamSchdules;
using Rafeek.Application.Handlers.ExamSchedules.Commands.UpdateExamSchdules;
using Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsSchedule;
using Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsScheduleById;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class ExamSchdulesController : BaseApiController
    {
        public ExamSchdulesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all exam schedules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.ExamSchedules.GetAll)]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Mentor), nameof(UserType.Professor))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetExamsScheduleQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get exam schedule by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.ExamSchedules.GetById)]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Mentor), nameof(UserType.Professor))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetExamsScheduleByIdQuery { Id = id });
            return Ok(result);
        }

        /// <summary>
        /// Create a new exam schedule
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.ExamSchedules.Create)]
        [RoleAuthorize(nameof(UserType.Admin))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateExamSchdulesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Update an existing exam schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.ExamSchedules.Update)]
        [RoleAuthorize(nameof(UserType.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExamSchdulesCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete an existing exam schedule
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ApiRoutes.ExamSchedules.Delete)]
        [RoleAuthorize(nameof(UserType.Admin))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromBody] DeleteExamSchdulesCommand command)
        {
            var result = await _mediator.Send(command);
            return Deleted(result);
        }
    }
}
