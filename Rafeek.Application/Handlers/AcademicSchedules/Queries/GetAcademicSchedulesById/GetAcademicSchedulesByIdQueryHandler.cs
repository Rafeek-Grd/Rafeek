using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicSchedules.DTOs;

namespace Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedulesById
{
    public class GetAcademicSchedulesByIdQueryHandler : IRequestHandler<GetAcademicSchedulesByIdQuery, AcademicScheduleDto>
    {
        private readonly IRafeekDbContext _context;

        public GetAcademicSchedulesByIdQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<AcademicScheduleDto> Handle(GetAcademicSchedulesByIdQuery request, CancellationToken cancellationToken)
        {
            var section = await _context.LectureGroups
                .AsNoTracking()
                .Include(s => s.Course)
                .Include(s => s.Doctor)
                    .ThenInclude(d => d.User)
                .Include(s => s.CalendarEvents)
                .FirstOrDefaultAsync(s => s.Id == request.LectureId, cancellationToken);

            if (section is null)
                return null!;

            var calendarEvent = section.CalendarEvents?.FirstOrDefault();

            string room = calendarEvent?.Location ?? "غير محدد";
            string status = calendarEvent?.Status.ToString() ?? "Published";

            return new AcademicScheduleDto
            {
                LectureGroupId = section.Id,
                CourseId = section.CourseId,
                CourseTitle = section.Course.Title,
                CourseCode = section.Course.Code,
                DoctorName = section.Doctor?.User?.FullName ?? "غير محدد",
                Room = room,
                Location = section.Location ?? room,
                Day = section.Day ?? "-",
                Time = section.Time ?? "-",
                Status = status
            };
        }
    }
}
