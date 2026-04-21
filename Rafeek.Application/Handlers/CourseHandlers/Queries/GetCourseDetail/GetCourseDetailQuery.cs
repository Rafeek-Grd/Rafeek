using MediatR;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail
{
    public class GetCourseDetailQuery : IRequest<CourseDetailDto>
    {
        /// <summary>معرّف المقرر</summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// معرّف الطالب (اختياري) — إذا أُرسل، سيُحسب حالة كل متطلب أساسي
        /// بناءً على تسجيلات هذا الطالب تحديداً.
        /// </summary>
        public Guid? StudentId { get; set; }
    }
}
