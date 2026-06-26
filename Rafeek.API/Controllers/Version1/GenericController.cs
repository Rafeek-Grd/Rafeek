using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class GenericController: BaseApiController
    {
        public GenericController(IMediator mediator):base(mediator)
        {
        }

        /// <summary>
        /// Get profiles for admins based on the provided query parameters.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Generic.GetProfilesForAdmins)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfilesGeneric([FromQuery] GetProfilesForAdminsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        } 
    }
}
