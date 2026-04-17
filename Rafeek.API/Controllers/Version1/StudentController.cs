using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.StudentHandlers.Commands;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class StudentController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route(ApiRoutes.Student.AssignToAcademicAdvisor)]
        public async Task<IActionResult> AssignToAcademicAdvisor([FromBody] AssignStudentToAcademicAdvisorCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return BadRequest("Failed to assign student to academic advisor. Please verify student and advisor IDs.");
            }

            return Ok("Student successfully assigned to academic advisor.");
        }

        [HttpPost]
        [Authorize]
        [Route(ApiRoutes.Student.RequestGuidance)]
        public async Task<IActionResult> RequestGuidance([FromBody] RequestGuidanceCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result, "Guidance request submitted successfully.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route(ApiRoutes.Student.GetDashboard)]
        public async Task<IActionResult> GetStudentDashboard([FromRoute] System.Guid userId)
        {
            try
            {
                var query = new Rafeek.Application.Handlers.StudentHandlers.Queries.GetStudentDashboard.GetStudentDashboardQuery(userId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
