using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetSecurityDashboard;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamResults;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamsSchedule;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetAcademicSchedules;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetStaffProfile;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentAcademicRecords;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentProfile;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetUserManagement;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class AdminController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// بيانات لوحة التحكم الرئيسية للأدمن:
        /// معدل مستوى الطلاب (رسم خطي)، توزيع الدفعات (دائري)،
        /// تحليل الحالة الأكاديمية، والعوائق الأكاديمية.
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Admin.GetDashboard)]
        [ProducesResponseType(typeof(AdminDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _mediator.Send(new GetAdminDashboardQuery());
            return Ok(result);
        }


        /// <summary>
        /// سجلات الطلاب الأكاديمية مع دعم الترقيم والفلترة والبحث.
        /// </summary>
        /// <param name="query">معاملات الفلترة والترقيم</param>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Admin.GetStudentAcademicRecords)]
        [ProducesResponseType(typeof(PagginatedResult<StudentAcademicRecordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudentAcademicRecords([FromQuery] GetStudentAcademicRecordsQuery query)
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
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
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
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin), nameof(UserType.Doctor))]
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
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
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
        /// جلب جميع الجداول الدراسية (المجموعات / المقاطع الدراسية) للوحة الإدارة
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Admin.GetAcademicSchedules)]
        [ProducesResponseType(typeof(List<AcademicScheduleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAcademicSchedules([FromQuery] GetAcademicSchedulesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// جلب جميع جداول الامتحانات مبوبة حسب الأيام
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Admin.GetExamsSchedule)]
        [ProducesResponseType(typeof(List<ExamDayGroupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetExamsSchedule([FromQuery] GetExamsScheduleQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// جلب نتائج الامتحانات (قابلة للفلترة حسب التبويبات كـ التخصص وغيرها)
        /// </summary>
        [HttpGet]
        [RoleAuthorize(nameof(UserType.Admin), nameof(UserType.SubAdmin))]
        [Route(ApiRoutes.Admin.GetExamResults)]
        [ProducesResponseType(typeof(List<ExamResultItemDto>), StatusCodes.Status200OK)]
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
    }
}
