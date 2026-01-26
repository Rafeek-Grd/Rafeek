using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AuthHandlers.SignUp;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticaionController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthenticaionController(IMediator mediator, IStringLocalizer<Messages> localizer ) : base(mediator,localizer)
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
    }
}
