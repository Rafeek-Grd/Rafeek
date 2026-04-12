using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.AddDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteDepartment;
using Rafeek.Application.Handlers.DepartmentHandlers.Commands.UpdateDepartment;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class DepartmentController: BaseApiController
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator): base(mediator) 
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add new department
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Department.Create)]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDepartment([FromBody] AddDepartmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Update Deparment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route(ApiRoutes.Department.Update)]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ApiRoutes.Department.Delete)]
        [RoleAuthorize(nameof(UserType.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var result = await _mediator.Send(new DeleteDepartmentCommand() { Id = id });
            return Ok(result);
        }
    }
}
