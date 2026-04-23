using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Commands.GradeAssignmentSubmission
{
    public class GradeAssignmentSubmissionCommandHandler : IRequestHandler<GradeAssignmentSubmissionCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public GradeAssignmentSubmissionCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(GradeAssignmentSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submission = await _ctx.AssignmentSubmissionRepository
                .GetAll(s => s.Id == request.SubmissionId)
                .FirstOrDefaultAsync(cancellationToken);

            submission!.Score = request.Score;
            submission.Feedback = request.Feedback;

            await _ctx.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
