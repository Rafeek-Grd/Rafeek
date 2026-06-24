using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAllStudentsGradesBatch
{
    public class GetAllStudentsGradesBatchQuery : IRequest<PagginatedResult<BatchStudentAIGradesDto>>
    {
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }
}
