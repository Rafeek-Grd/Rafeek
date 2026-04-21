using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetAcademicSchedules
{
    public class GetAcademicSchedulesQueryHandler : IRequestHandler<GetAcademicSchedulesQuery, List<AcademicScheduleDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetAcademicSchedulesQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<List<AcademicScheduleDto>> Handle(GetAcademicSchedulesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Sections
                .AsNoTracking()
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                    .ThenInclude(i => i.User)
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
                    SectionId = section.Id,
                    CourseTitle = section.Course.Title,
                    CourseCode = section.Course.Code,
                    InstructorName = section.Instructor?.User?.FullName ?? "غير محدد",
                    Room = room,
                    Day = section.Day ?? "-",
                    Time = section.Time ?? "-",
                    Status = status
                });
            }

            return schedules;
        }
    }
}
