using MediatR;
using Rafeek.Application.Common.Models.AI;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAllStudentsGradesBatch
{
    public class GetAllStudentsGradesBatchQuery : IRequest<List<BatchStudentAIGradesDto>>
    {
    }
}
