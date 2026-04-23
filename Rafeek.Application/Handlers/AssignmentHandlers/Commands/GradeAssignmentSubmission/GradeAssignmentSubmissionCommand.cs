using MediatR;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Commands.GradeAssignmentSubmission
{
    public class GradeAssignmentSubmissionCommand : IRequest<Unit>
    {
        public Guid SubmissionId { get; set; }
        public float Score { get; set; }
        public string? Feedback { get; set; }
    }
}
