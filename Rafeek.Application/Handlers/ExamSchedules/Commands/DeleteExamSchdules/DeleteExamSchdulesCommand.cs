using MediatR;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.DeleteExamSchdules
{
    public class DeleteExamSchdulesCommand : IRequest<string>
    {
        public Guid? Id { get; set; }
    }
}