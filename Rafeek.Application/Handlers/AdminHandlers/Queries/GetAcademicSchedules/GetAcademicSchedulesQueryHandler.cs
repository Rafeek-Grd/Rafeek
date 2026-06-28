using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetAcademicSchedules
{
    public class GetAcademicSchedulesQueryHandler : IRequestHandler<GetAcademicSchedulesQuery, PagginatedResult<AcademicScheduleDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetAcademicSchedulesQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<PagginatedResult<AcademicScheduleDto>> Handle(GetAcademicSchedulesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.LectureGroups
                .AsNoTracking()
                .Include(s => s.Course)
                .Include(s => s.Doctor)
                    .ThenInclude(d => d.User)
                .Include(s => s.CalendarEvents)
                .AsQueryable();

            if (request.TermId.HasValue)
            {
                query = query.Where(s => s.CalendarEvents.Any(ce => ce.AcademicTermId == request.TermId.Value));
            }

            var sections = await query.ToListAsync(cancellationToken);

            var schedules = new List<AcademicScheduleDto>();

            foreach (var section in sections)
            {
                var calendarEvent = section.CalendarEvents?.FirstOrDefault();
                
                string room = calendarEvent?.Location ?? "غير محدد";
                string status = calendarEvent?.Status.ToString() ?? "Published";

                schedules.Add(new AcademicScheduleDto
                {
                    LectureGroupId = section.Id,
                    CourseId = section.CourseId,
                    CourseTitle = section.Course.Title,
                    CourseCode = section.Course.Code,
                    InstructorName = section.Doctor?.User?.FullName ?? "غير محدد",
                    Room = room,
                    Location = section.Location ?? room,
                    Day = section.Day ?? "-",
                    Time = section.Time ?? "-",
                    Status = status
                });
            }

            var totalCount = schedules.Count;
            List<AcademicScheduleDto> items;

            if (request.PageNumber == -1)
            {
                items = schedules;
            }
            else
            {
                items = schedules
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();
            }

            return new PagginatedResult<AcademicScheduleDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
