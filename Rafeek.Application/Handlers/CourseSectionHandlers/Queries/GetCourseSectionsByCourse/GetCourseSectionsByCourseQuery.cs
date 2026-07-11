using MediatR;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Queries.GetCourseSectionsByCourse
{
    public class GetCourseSectionsByCourseQuery : IRequest<List<CourseSectionDto>>
    {
        public Guid CourseId { get; set; }
    }
}
