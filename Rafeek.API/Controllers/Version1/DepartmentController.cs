using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.AddDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.UpdateDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignCourseToDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignUserToDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteCourseFromDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteUserFromDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllDepartmentsPagginated;
using Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllUsersInDepartmentPagginated;
using Rafeek.Application.Handlers.DepartmentHandlers.Query.GetDepartmentByIdOrCode;
using Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllCoursesInDepartmentPagginated;
using Rafeek.Domain.Enums;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class DepartmentController: BaseApiController
    {
        public DepartmentController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer) 
        {
        }


        /// <summary>
        /// Add new department
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Department.Create)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDepartment([FromBody] AddDepartmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(ApiRoutes.Department.GetByIdOrCode, result);
        }


        /// <summary>
        /// Update Deparment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Department.Update)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Accepted(result);
        }


        /// <summary>
        /// Delete Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Department.Delete)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var result = await _mediator.Send(new DeleteDepartmentCommand() { Id = id });
            return Deleted(result);
        }


        /// <summary>
        /// Assign course to department.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Department.AssignCourse)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignCourse([FromBody] AssignCourseToDepartmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Accepted(result);
        }


        /// <summary>
        /// Assign user to department.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Department.AssignUser)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignUser([FromBody] AssignUserToDepartmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Accepted(result);
        }


        /// <summary>
        /// Unassign course from department.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Department.RemoveCourse)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveCourse([FromRoute] Guid courseId)
        {
            var result = await _mediator.Send(new DeleteCourseFromDepartmentCommand { CourseId = courseId });
            return Deleted(result);
        }


        /// <summary>
        /// Unassign user from department.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Department.RemoveUser)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUser([FromBody] DeleteUserFromDepartmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Deleted(result);
        }


        /// <summary>
        /// Get all departments paginated.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin), nameof(UserType.Staff), nameof(UserType.Doctor))]
        [Route(ApiRoutes.Department.GetAllPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPagginated([FromQuery] GetAllDepartmentsPagginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get all users in a department paginated.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Department.GetAllUsersInDepartmentPagginated)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers([FromRoute] Guid id, [FromQuery] GetAllUsersInDepartmentPagginatedQuery query)
        {
            query.DepartmentId = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get department by id or code.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin), nameof(UserType.Staff), nameof(UserType.Doctor))]
        [Route(ApiRoutes.Department.GetByIdOrCode)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdOrCode([FromQuery] GetDepartmentByIdOrCodeQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get all courses in a department paginated.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin), nameof(UserType.Staff), nameof(UserType.Doctor))]
        [Route(ApiRoutes.Department.GetAllCoursesInDepartment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCourses([FromRoute] Guid id, [FromQuery] GetAllCoursesInDepartmentPagginatedQuery query)
        {
            query.DepartmentId = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
