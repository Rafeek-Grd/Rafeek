using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetAllCoursesPaginated
{
    public class GetAllCoursesPaginatedQuery : IRequest<PagginatedResult<CourseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchTerm { get; set; }
    }
}
