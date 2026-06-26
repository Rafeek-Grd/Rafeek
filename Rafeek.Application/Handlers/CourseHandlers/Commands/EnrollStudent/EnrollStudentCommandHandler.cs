using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.EnrollStudent
{
    public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, bool>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public EnrollStudentCommandHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);
            
            if (student == null) throw new UnauthorizedException("Student profile not found.");

            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == request.CourseId, cancellationToken);

            if (existingEnrollment != null) throw new BadRequestException("Already enrolled in this course.");

            var lectureGroup = await _context.LectureGroups
                .FirstOrDefaultAsync(s => s.Id == request.LectureGroupId && s.CourseId == request.CourseId, cancellationToken);

            if (lectureGroup == null) throw new NotFoundException(nameof(LectureGroup), request.LectureGroupId);

            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseId = request.CourseId,
                LectureGroupId = request.LectureGroupId,
                Status = "Active"
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
