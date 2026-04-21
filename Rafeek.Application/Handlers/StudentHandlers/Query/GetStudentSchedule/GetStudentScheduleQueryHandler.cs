using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentSchedule
{
    public class GetStudentScheduleQueryHandler : IRequestHandler<GetStudentScheduleQuery, List<ScheduleItemDto>>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetStudentScheduleQueryHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<ScheduleItemDto>> Handle(GetStudentScheduleQuery request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);
            
            if (student == null) throw new UnauthorizedException("Student profile not found.");

            var schedule = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Section)
                .Where(e => e.StudentId == student.Id)
                .Select(e => new ScheduleItemDto
                {
                    CourseId = e.CourseId,
                    CourseCode = e.Course.Code,
                    CourseTitle = e.Course.Title,
                    SectionId = e.SectionId,
                    Day = e.Section.Day,
                    Time = e.Section.Time,
                    Status = e.Status
                })
                .ToListAsync(cancellationToken);

            return schedule;
        }
    }
}
