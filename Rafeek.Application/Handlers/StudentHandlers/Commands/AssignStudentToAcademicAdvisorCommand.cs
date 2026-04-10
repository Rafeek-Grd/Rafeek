using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands
{
    public class AssignStudentToAcademicAdvisorCommand : IRequest<bool>
    {
        public Guid StudentId { get; set; }
        public Guid AcademicAdvisorId { get; set; }
    }

    public class AssignStudentToAcademicAdvisorCommandHandler : IRequestHandler<AssignStudentToAcademicAdvisorCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public AssignStudentToAcademicAdvisorCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AssignStudentToAcademicAdvisorCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == request.StudentId, cancellationToken);

            if (student == null)
            {
                return false;
            }

            var doctorExists = await _context.Doctors
                .AnyAsync(x => x.Id == request.AcademicAdvisorId, cancellationToken);
            
            if (!doctorExists)
            {
                // Returning false if advisor doesn't exist, could also throw an exception
                return false; 
            }

            student.AcademicAdvisorId = request.AcademicAdvisorId;
            
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
