using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetSecurityDashboard;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamResults;
using Rafeek.Application.Handlers.ExamSchedules.DTOs;
using Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsSchedule;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetStaffProfile;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentProfile;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetUserManagement;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using Rafeek.Application.Handlers.SettingsHandlers.Commands.UpdateAcademicSettings;
using Rafeek.Application.Handlers.SettingsHandlers.Queries.GetAcademicSettings;
using Rafeek.Application.Handlers.SettingsHandlers.DTOs;
using Rafeek.Application.Handlers.AdminHandlers.Commands.UpdateSecurityDashboard;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AdminController : BaseApiController
    {

        public AdminController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
        }


        /// <summary>
        /// بيانات لوحة التحكم الرئيسية للأدمن:
        /// معدل مستوى الطلاب (رسم خطي)، توزيع الدفعات (دائري)،
        /// تحليل الحالة الأكاديمية، والعوائق الأكاديمية.
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.GetDashboard)]
        [ProducesResponseType(typeof(AdminDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDashboard([FromQuery] GetAdminDashboardQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }




        /// <summary>
        /// قائمة إدارة المستخدمين: تدعم التبويبات (الطلاب، طاقم التدريس، إلخ) عبر المعامل TabRole.
        /// تدعم البحث، والفلترة حسب القسم، والحالة.
        /// </summary>
        /// <param name="query">معاملات الفلترة التي تشمل TabRole (نوع المستخدم)</param>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.GetUsers)]
        [ProducesResponseType(typeof(PagginatedResult<UserManagementListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers([FromQuery] GetUserManagementQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// جلب الملف الشخصي للطالب (للمشرفين) متضمناً المعلومات الأساسية،
        /// المقررات المسجلة حالياً، وتاريخ النتائج.
        /// </summary>
        /// <param name="studentId">معرّف الطالب</param>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.Professor))]
        [Route(ApiRoutes.Admin.GetStudentProfile)]
        [ProducesResponseType(typeof(AdminStudentProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudentProfile([FromRoute] Guid studentId)
        {
            var query = new GetAdminStudentProfileQuery { StudentId = studentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// جلب الملف الشخصي لطاقم التدريس (المعيدين، والدكاترة) متضمناً
        /// المعلومات الشخصية، ساعات العمل، ومقررات التدريس.
        /// </summary>
        /// <param name="userId">معرّف الموظف (ApplicationUser ID)</param>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.GetStaffProfile)]
        [ProducesResponseType(typeof(AdminStaffProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStaffProfile([FromRoute] Guid userId)
        {
            var query = new GetAdminStaffProfileQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// جلب نتائج الامتحانات (قابلة للفلترة حسب التبويبات كـ التخصص وغيرها)
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.GetExamResults)]
        [ProducesResponseType(typeof(PagginatedResult<ExamResultItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetExamResults([FromQuery] GetExamResultsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// جلب جميع إحصائيات وإعدادات مركز الأمان والمراقبة
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.GetSecurityDashboard)]
        [ProducesResponseType(typeof(SecurityDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSecurityDashboard([FromQuery] GetSecurityDashboardQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
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

        /// <summary>
        /// تحديث الإعدادات الأمنية وصلاحيات الأدوار.
        /// </summary>
        [HttpPut]
        [RoleAuthorize(nameof(UserType.Admin))]
        [Route(ApiRoutes.Admin.UpdateSecurityDashboard)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateSecurityDashboard([FromBody] UpdateSecurityDashboardCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return result ? Ok(new { message = "تم تحديث إعدادات الأمان بنجاح" }) : BadRequest();
        }
    }
}
