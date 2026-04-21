using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.CourseHandlers.Commands.CreateNewCourse;
using Rafeek.Application.Handlers.CourseHandlers.Commands.DeleteCourse;
using Rafeek.Application.Handlers.CourseHandlers.Commands.UpdateCourse;
using Rafeek.Application.Handlers.CourseHandlers.Commands.DropCourse;
using Rafeek.Application.Handlers.CourseHandlers.Commands.EnrollStudent;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourses;
using Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class CourseController : BaseApiController
    {
        public CourseController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateNewCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Updates existing course details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete the course with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCourseCommand { Id = id });
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all courses with pagination and optional search filter.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.GetAllPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPagginated([FromQuery] GetAllCoursesPaginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get All Courses Pagginated
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Courses.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCoursesPagginated([FromQuery] GetCoursesQueryPagginated query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single course by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCourseByIdQuery { Id = id });
            return Ok(result);
        }

        /// <summary>
        /// Enrolls a student in a specific course section.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.Enroll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Enroll([FromBody] EnrollStudentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Drops a student from a course.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.Course.Drop)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Drop([FromBody] DropCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get Course Details.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Courses.GetDetail)]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseDetail([FromRoute] Guid courseId,[FromQuery] Guid? studentId)
        {
            var result = await _mediator.Send(new GetCourseDetailQuery
            {
                CourseId = courseId,
                StudentId = studentId
            });
            return Ok(result);
        }
    }
}
