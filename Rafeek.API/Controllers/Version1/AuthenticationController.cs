using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AuthHandlers.RefreshToken;
using Rafeek.Application.Handlers.AuthHandlers.SignIn;
using Rafeek.Application.Handlers.AuthHandlers.SignUp;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticationController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route(ApiRoutes.Authentication.SignUp)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Authentication.SignIn)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Authentication.RefreshToken)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
