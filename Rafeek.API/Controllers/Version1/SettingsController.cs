using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.SettingsHandlers.Commands.UpdateAcademicSettings;
using Rafeek.Application.Handlers.SettingsHandlers.Queries.GetAcademicSettings;
using Rafeek.Application.Handlers.SettingsHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class SettingsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get general academic settings and grading scales.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.GetSettings)]
        [ProducesResponseType(typeof(AcademicSettingsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _mediator.Send(new GetAcademicSettingsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Update academic settings and grading scales.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.UpdateSettings)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateAcademicSettingsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
