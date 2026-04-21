using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudyPlanHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudyPlanHandlers.Queries.GetStudyPlanByStudent
{
    public class GetStudyPlanByStudentQueryPagginated : IRequest<PagginatedResult<StudyPlanDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public Guid StudentId { get; set; }
    }
}
