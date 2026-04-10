using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands
{
    public class ReviewStudentGuidanceRequestCommand : IRequest<bool>
    {
        public Guid AdvisorId { get; set; }
        public Guid RequestId { get; set; }
        public StudentSupportStatus Status { get; set; }
    }

    public class ReviewStudentGuidanceRequestCommandHandler : IRequestHandler<ReviewStudentGuidanceRequestCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public ReviewStudentGuidanceRequestCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ReviewStudentGuidanceRequestCommand request, CancellationToken cancellationToken)
        {
            // Verify and retrieve the request. We also include the student to verify the advisor.
            var supportRequest = await _context.StudentSupports
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == request.RequestId, cancellationToken);

            if (supportRequest == null)
            {
                throw new Exception("Guidance request not found.");
            }

            if (supportRequest.Student.AcademicAdvisorId != request.AdvisorId)
            {
                throw new Exception("You are not the designated advisor for this student.");
            }

            supportRequest.StudentSupportStatus = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
