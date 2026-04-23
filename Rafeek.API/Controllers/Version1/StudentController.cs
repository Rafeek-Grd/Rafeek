using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.API.Filters;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.StudentHandlers.Commands.SendRequestForAdvismentGuide;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentDashboard;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentProfile;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentSchedule;
using Rafeek.Application.Handlers.StudentHandlers.Query.GetChatHistory;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class StudentController : BaseApiController
    {

        public StudentController(IMediator mediator, IStringLocalizer<Messages> localizer) 
            : base(mediator, localizer)
        {
        }

        /// <summary>
        /// Send request for guidance to an advisor, including the title and description of the request.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.Student.SendRequestToGuide)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendRequestForGuidance([FromBody] SendRequestForAdvismentGuideCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(ApiRoutes.Advisor.GetAllGuidanceRequestsPagginated, result);
        }

        /// <summary>
        /// جلب البيانات المبدئية لنموذج حجز خدمة أكاديمية (بيانات الطالب وتفاصيل المشرف).
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.Student.GetAcademicServiceInitialData)]
        [ProducesResponseType(typeof(Rafeek.Application.Handlers.StudentHandlers.Query.GetAcademicServiceInitialData.AcademicServiceInitialDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAcademicServiceInitialData()
        {
            var result = await _mediator.Send(new Rafeek.Application.Handlers.StudentHandlers.Query.GetAcademicServiceInitialData.GetAcademicServiceInitialDataQuery());
            return Ok(result);
        }

        /// <summary>
        /// حجز خدمة أكاديمية (موعد مع المشرف الأكاديمي).
        /// تفاصيل المشرف يتم سحبها تلقائياً للملء.
        /// </summary>
        [HttpPost]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.Student.BookAcademicService)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookAcademicService([FromBody] Rafeek.Application.Handlers.StudentHandlers.Commands.BookAcademicService.BookAcademicServiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, result);
        }


        /// <summary>
        /// Get the profile of the currently logged-in student, including academic history.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Student.GetProfile)]
        [ProducesResponseType(typeof(StudentProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetStudentProfileQuery());
            return Ok(result);
        }


        /// <summary>
        /// Get Student Dashboard data.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Student.GetDashboard)]
        [ProducesResponseType(typeof(StudentDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentDashboard([FromRoute] Guid userId)
        {
            var query = new GetStudentDashboardQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get Student Enrollments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Student))]
        [Route(ApiRoutes.Student.GetSchedule)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentSchedule()
        {
            var result = await _mediator.Send(new GetStudentScheduleQuery());
            return Ok(result);
        }
    }
}

