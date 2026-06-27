using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.StaffHandlers.GetStaffDashboard;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class StaffController: BaseApiController
    {
        public StaffController(IMediator mediator): base(mediator)
        {
        }

        /// <summary>
        /// Get Staff Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Staff.GetStaffDashboard)]
        [RoleAuthorize(nameof(UserType.Staff))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStaffDashboard()
        {
            var result = await _mediator.Send(new GetStaffDashboardQuery());
            return Ok(result);
        }
    }
}
