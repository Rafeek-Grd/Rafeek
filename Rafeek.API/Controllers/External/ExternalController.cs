using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AIHandlers.Queries.GetAllStudentsGradesBatch;
using Rafeek.Application.Handlers.AIHandlers.Queries.GetCourseCatalogMetadata;
using Rafeek.Application.Handlers.AIHandlers.Queries.GetStudentGrades;
using Rafeek.Application.Localization;
using Rafeek.API.Filters;

namespace Rafeek.API.Controllers.External
{
    [ApiKey]
    [ApiVersion("1.0")]
    public class ExternalController : BaseApiController
    {
        public ExternalController(IMediator mediator, IStringLocalizer<Messages> localizer) 
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Get a specific student's grades and performance metrics for AI-driven personalized learning and support.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route(ApiRoutes.ExternalIntegration.GetStudentGrades)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudentGrades(Guid studentId)
        {
            var result = await _mediator.Send(new GetStudentGradesQuery { StudentId = studentId});
            return Ok(result);
        }

        /// <summary>
        /// Get batch dump of all students' grades for AI analytics and model training.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Route(ApiRoutes.ExternalIntegration.GetBatchDump)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBatchDump()
        {
            var result = await _mediator.Send(new GetAllStudentsGradesBatchQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get Course Catalog metadata for AI-driven course recommendations and curriculum analysis.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Route(ApiRoutes.ExternalIntegration.GetCatalog)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCatalog()
        {
            var result = await _mediator.Send(new GetCourseCatalogMetadataQuery());
            return Ok(result);
        }
    }
}
