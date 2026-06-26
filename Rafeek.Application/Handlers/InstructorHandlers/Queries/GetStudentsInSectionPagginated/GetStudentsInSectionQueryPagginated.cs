using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentsInSection
{
    public class GetStudentsInSectionQueryPagginated : IRequest<PagginatedResult<SectionStudentDto>>
    {
        public Guid LectureGroupId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
