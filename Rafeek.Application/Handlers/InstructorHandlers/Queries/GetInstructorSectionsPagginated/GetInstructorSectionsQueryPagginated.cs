using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorSections
{
    public class GetInstructorSectionsQueryPagginated : IRequest<PagginatedResult<InstructorSectionDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
