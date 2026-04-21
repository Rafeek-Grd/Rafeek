using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourses
{
    public class GetCoursesQueryPagginated : IRequest<PagginatedResult<CourseListItemDto>>
    {
        public string? SearchTerm { get; set; }
        public int? Semester { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? Status { get; set; }
        public int? CreditHours { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
