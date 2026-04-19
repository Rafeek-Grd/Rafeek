using MediatR;
using Rafeek.Application.Common.Models.AI;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetStudentGrades
{
    public class GetStudentGradesQuery : IRequest<StudentAIGradesDto>
    {
        public Guid StudentId { get; set; }
    }
}
