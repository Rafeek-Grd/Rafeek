using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AuthHandlers.Commands.ActivateUniversityEmail;
using Rafeek.Application.Handlers.AuthHandlers.Commands.CheckFromConfirmationCode;
using Rafeek.Application.Handlers.AuthHandlers.Commands.ForegetPassword;
using Rafeek.Application.Handlers.AuthHandlers.Commands.RefreshToken;
using Rafeek.Application.Handlers.AuthHandlers.Commands.ResetPassword;
using Rafeek.Application.Handlers.AuthHandlers.Commands.SignIn;
using Rafeek.Application.Handlers.AuthHandlers.Commands.SignUp;
using Rafeek.Application.Handlers.AuthHandlers.Query;
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

        /// <summary>
        /// Only Admins can create new users.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Authentication.SignUp)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Sign in with email and password.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Authentication.SignIn)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Refresh the access token using the refresh token.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Authentication.RefreshToken)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Reset the user password using the token received from forgot-password.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Authentication.ResetPassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Forgot password, send an email with a link to reset the password.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Authentication.ForgotPassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForegetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Check if the confirmation code is valid for the given email.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Authentication.CheckFromConfirmationCode)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckFromConfirmationCode([FromBody] CheckFromConfirmationCodeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Activate university email for a user. Only admins can activate university emails.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Authentication.ActivateUniversityEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivateUniversityEmail([FromBody] ActivateUniversityEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get Current User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.Authentication.GetUserProfile)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _mediator.Send(new GetUserProfileQuery());
            return Ok(result);
        }
    }
}
