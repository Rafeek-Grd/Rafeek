using MediatR;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetStudentGrades
{
    public class GetStudentGradesQuery : IRequest<StudentAIGradesDto>
    {
        public Guid StudentId { get; set; }
    }
}
