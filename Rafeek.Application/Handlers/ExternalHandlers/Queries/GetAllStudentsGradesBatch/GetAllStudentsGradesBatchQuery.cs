using MediatR;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAllStudentsGradesBatch
{
    public class GetAllStudentsGradesBatchQuery : IRequest<List<BatchStudentAIGradesDto>>
    {
    }
}
