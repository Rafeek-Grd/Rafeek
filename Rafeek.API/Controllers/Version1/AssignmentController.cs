using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AssignmentHandlers.Commands.CreateAssignment;
using Rafeek.Application.Handlers.AssignmentHandlers.Commands.GradeAssignmentSubmission;
using Rafeek.Application.Handlers.AssignmentHandlers.Queries.GetAssignmentsBySection;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AssignmentController : BaseApiController
    {
        public AssignmentController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Get Assignments by section with pagination.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Assignments.GetBySection)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBySection([FromRoute] Guid sectionId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetAssignmentsBySectionQueryPaginated()
            {
                SectionId = sectionId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create assignment.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Instructor), nameof(UserType.Doctor))]
        [Route(ApiRoutes.Assignments.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Grade Assignment submission.
        /// </summary>
        /// <param name="submissionId"></param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize(nameof(UserType.Instructor), nameof(UserType.Doctor))]
        [Route(ApiRoutes.Assignments.GradeSubmission)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GradeSubmission([FromRoute] Guid submissionId, [FromBody] GradeAssignmentSubmissionCommand command, CancellationToken cancellationToken)
        {
            command.SubmissionId = submissionId;
            await _mediator.Send(command, cancellationToken);
            return Ok<object>(null);
        }
    }
}
