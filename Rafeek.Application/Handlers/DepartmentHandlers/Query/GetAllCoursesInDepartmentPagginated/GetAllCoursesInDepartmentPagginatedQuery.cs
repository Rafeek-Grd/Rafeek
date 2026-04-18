using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllCoursesInDepartmentPagginated
{
    public class GetAllCoursesInDepartmentPagginatedQuery : IRequest<PagginatedResult<CourseDto>>
    {
        public Guid DepartmentId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
