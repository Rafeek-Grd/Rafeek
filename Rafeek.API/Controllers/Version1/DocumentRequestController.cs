using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.DocumentHandlers.Commands.CreateDocumentRequest;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class DocumentRequestController : BaseApiController
    {
        public DocumentRequestController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Create a new document request for the authenticated student.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize]
        [Route(ApiRoutes.DocumentRequest.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDocumentRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
