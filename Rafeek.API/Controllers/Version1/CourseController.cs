using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Filters;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail;
using Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourses;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    public class CourseController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// قائمة المقررات مع دعم الفلترة (القسم، الفصل، الحالة، الساعات) والبحث والترقيم.
        /// تُستخدم لعرض بطاقات المقررات في شاشة "إدارة المقررات".
        /// </summary>
        /// <param name="query">معاملات الفلترة والترقيم</param>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Courses.GetAll)]
        [ProducesResponseType(typeof(PagginatedResult<CourseListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCourses([FromQuery] GetCoursesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        /// <summary>
        /// تفاصيل مقرر واحد: نظرة عامة، المتطلبات الأساسية، الخطة الدراسية،
        /// مدرسو المادة، والإشعارات المرتبطة بالمقرر.
        /// مرر studentId (اختياري) لحساب حالة كل متطلب أساسي لهذا الطالب تحديداً.
        /// </summary>
        /// <param name="courseId">معرّف المقرر</param>
        /// <param name="studentId">معرّف الطالب (اختياري)</param>
        [HttpGet]
        [RoleAuthorize]
        [Route(ApiRoutes.Courses.GetDetail)]
        [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCourseDetail(
            [FromRoute] Guid courseId,
            [FromQuery] Guid? studentId)
        {
            var query = new GetCourseDetailQuery
            {
                CourseId  = courseId,
                StudentId = studentId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
