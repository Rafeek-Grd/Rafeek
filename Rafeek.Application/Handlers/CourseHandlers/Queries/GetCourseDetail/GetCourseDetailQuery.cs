using MediatR;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail
{
    public class GetCourseDetailQuery : IRequest<CourseDetailDto>
    {
        public Guid CourseId { get; set; }
        public Guid? StudentId { get; set; }
    }
}
