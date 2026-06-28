using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentSchedule
{
    public class GetStudentScheduleQueryHandler : IRequestHandler<GetStudentScheduleQuery, PagginatedResult<ScheduleItemDto>>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetStudentScheduleQueryHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<ScheduleItemDto>> Handle(GetStudentScheduleQuery request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);

            if (student == null) throw new UnauthorizedException("Student profile not found.");

            var query = _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .Include(e => e.LectureGroup)
                .Where(e => e.StudentId == student.Id)
                .Select(e => new ScheduleItemDto
                {
                    CourseId = e.CourseId,
                    CourseCode = e.Course.Code,
                    CourseTitle = e.Course.Title,
                    LectureGroupId = e.LectureGroupId,
                    Day = e.LectureGroup.Day,
                    Time = e.LectureGroup.Time,
                    Location = e.LectureGroup.Location,
                    Status = e.Status
                });

            List<ScheduleItemDto> items;
            int totalCount;

            if (request.PageNumber == -1)
            {
                items = await query.ToListAsync(cancellationToken);
                totalCount = items.Count;
            }
            else
            {
                totalCount = await query.CountAsync(cancellationToken);
                items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }

            return new PagginatedResult<ScheduleItemDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
