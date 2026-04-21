using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<PagginatedResult<CourseListItemDto>>
    {
        /// <summary>البحث برمز المقرر أو العنوان</summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// الفصل الدراسي — القيم من Enum: 0=First | 1=Second | ... | 7=Eighth
        /// مثال Fall 2024 يُمثَّل بالرقم المناظر.
        /// اتركه فارغاً لإرجاع جميع الفصول.
        /// </summary>
        public int? Semester { get; set; }

        /// <summary>فلتر بمعرّف القسم</summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// فلتر الحالة — القيم: "Open" | "Full" | "Closed"
        /// اتركه فارغاً لإرجاع جميع الحالات.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>فلتر عدد الساعات المعتمدة</summary>
        public int? CreditHours { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
