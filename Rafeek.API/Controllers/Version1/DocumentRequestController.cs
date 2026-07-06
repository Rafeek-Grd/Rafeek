using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DocumentHandlers.Commands.CreateDocumentRequest;
using Rafeek.Application.Handlers.DocumentHandlers.Commands.UpdateDocumentRequestStatus;
using Rafeek.Application.Handlers.DocumentHandlers.Commands.BulkUpdateDocumentRequestStatus;
using Rafeek.Application.Handlers.DocumentHandlers.Queries.GetDocumentRequestsPaginated;
using Rafeek.Application.Handlers.DocumentHandlers.Queries.ExportDocumentRequests;
using Rafeek.Application.Handlers.DocumentHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using System;
using System.Threading.Tasks;
using Rafeek.Application.Handlers.DocumentHandlers.Commands.DeleteDocumentRequest;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class DocumentRequestController : BaseApiController
    {
        public DocumentRequestController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Create a new document/academic request for the authenticated student.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.DocumentRequest.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDocumentRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Get all document/academic requests with pagination, filtering, and searching capabilities.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff), nameof(UserType.Professor))]
        [Route(ApiRoutes.DocumentRequest.GetAll)]
        [ProducesResponseType(typeof(PagginatedResult<DocumentRequestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetDocumentRequestsPaginatedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Update status of a specific document/academic request (approve, reject, etc.).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff), nameof(UserType.Professor))]
        [Route(ApiRoutes.DocumentRequest.UpdateStatus)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateDocumentRequestStatusCommand command)
        {
            command.RequestId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Bulk update status of multiple document/academic requests.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.DocumentRequest.BulkUpdateStatus)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] BulkUpdateDocumentRequestStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Export document/academic requests to a CSV file.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.DocumentRequest.Export)]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Export([FromQuery] ExportDocumentRequestsQuery query)
        {
            var result = await _mediator.Send(query);
            return File(result, "text/csv", $"academic_requests_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
        }

        /// <summary>
        /// Delete a specific document/academic request by its ID.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Staff))]
        [Route(ApiRoutes.DocumentRequest.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromQuery]DeleteDocumentRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
