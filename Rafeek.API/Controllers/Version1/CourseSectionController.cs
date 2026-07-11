using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.CourseSectionHandlers.Commands.CreateCourseSection;
using Rafeek.Application.Handlers.CourseSectionHandlers.Commands.UpdateCourseSection;
using Rafeek.Application.Handlers.CourseSectionHandlers.Commands.DeleteCourseSection;
using Rafeek.Application.Handlers.CourseSectionHandlers.Queries.GetCourseSectionsByCourse;
using Rafeek.Application.Localization;
using Rafeek.API.Filters;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class CourseSectionController : BaseApiController
    {
        public CourseSectionController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
        }

        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.CourseSection.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCourseSectionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [RoleAuthorize]
        [Route(ApiRoutes.CourseSection.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseSectionCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [RoleAuthorize]
        [Route(ApiRoutes.CourseSection.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCourseSectionCommand { Id = id });
            if (!result) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.CourseSection.GetByCourse)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByCourse(Guid courseId)
        {
            var result = await _mediator.Send(new GetCourseSectionsByCourseQuery { CourseId = courseId });
            return Ok(result);
        }
    }
}
