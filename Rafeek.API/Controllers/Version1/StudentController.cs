using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.API.Filters;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.StudentHandlers.Commands.SendRequestForAdvismentGuide;
using Rafeek.Application.Handlers.StudentHandlers.Query;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentProfile;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class StudentController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Send request for guidance to an advisor, including the title and description of the request.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.Student.SendRequestToGuide)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendRequestForGuidance([FromBody] SendRequestForAdvismentGuideCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get the profile of the currently logged-in student, including academic history.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize()]
        [Route(ApiRoutes.Student.GetProfile)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetStudentProfileQuery());
            return Ok(result);
        }
    }
}
