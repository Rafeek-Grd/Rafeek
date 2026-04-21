using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.DropCourse
{
    public class DropCourseCommandHandler : IRequestHandler<DropCourseCommand, bool>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DropCourseCommandHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DropCourseCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);
            
            if (student == null) throw new UnauthorizedException("Student profile not found.");

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == request.CourseId, cancellationToken);

            if (enrollment == null) throw new NotFoundException("Enrollment not found for this course.");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
