using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.InstructorHandlers.Commands.SubmitSectionGrades;
using Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorDashboard;
using Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorExamSchedule;
using Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorNotifications;
using Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorSections;
using Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentProfileForInstructor;
using Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentsInSection;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    [RoleAuthorize(nameof(UserType.Instructor), nameof(UserType.Doctor))]
    public class InstructorController : BaseApiController
    {
        public InstructorController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Get Instructor Dashboard data including total sections, total students, and pending assignments to grade.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Instructor.GetDashboard)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _mediator.Send(new GetInstructorDashboardQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get Instructor Sections with pagginated.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Instructor.GetSections)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSections()
        {
            var result = await _mediator.Send(new GetInstructorSectionsQueryPagginated());
            return Ok(result);
        }

        /// <summary>
        /// Get Students In section by sectionId with pagination.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Instructor.GetSectionStudents)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSectionStudents([FromRoute] Guid sectionId)
        {
            var result = await _mediator.Send(new GetStudentsInSectionQueryPagginated { SectionId = sectionId });
            return Ok(result);
        }

        /// <summary>
        /// Get Student Profile for instructor.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Instructor.GetStudentProfile)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentProfile([FromRoute] Guid studentId)
        {
            var result = await _mediator.Send(new GetStudentProfileForInstructorQuery { StudentId = studentId });
            return Ok(result);
        }

        /// <summary>
        /// Submit grades for a section.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Instructor.SubmitGrades)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitGrades([FromRoute] Guid sectionId, [FromBody] SubmitSectionGradesCommand command)
        {
            command.SectionId = sectionId;
            await _mediator.Send(command);
            return Ok<object>(null);
        }

        /// <summary>
        /// Get instructor exam schedule with pagination.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Instructor.GetExamSchedule)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExamSchedule()
        {
            var result = await _mediator.Send(new GetInstructorExamScheduleQueryPagginated());
            return Ok(result);
        }

        /// <summary>
        /// Get instructor notifications with pagination.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Instructor.GetNotifications)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _mediator.Send(new GetInstructorNotificationsQueryPagginated());
            return Ok(result);
        }
    }
}
