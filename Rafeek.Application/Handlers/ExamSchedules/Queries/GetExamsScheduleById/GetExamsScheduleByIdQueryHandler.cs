using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Handlers.ExamSchedules.DTOs;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsScheduleById
{
    public class GetExamsScheduleByIdQueryHandler : IRequestHandler<GetExamsScheduleByIdQuery, ExamItemDto>
    {
        private readonly IRafeekDbContext _context;

        public GetExamsScheduleByIdQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<ExamItemDto> Handle(GetExamsScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var exam = await _context.AcademicCalendars
                .AsNoTracking()
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == request.Id && e.EventType == AcademicCalendarEventType.Exam, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.AcademicCalendar), request.Id);

            return new ExamItemDto
            {
                ExamId = exam.Id,
                TimeLabel = exam.StartTime.ToString(@"hh\:mm"),
                DurationLabel = $"{(exam.EndTime - exam.StartTime).TotalHours:0.#} ساعة",
                CourseCode = exam.Course?.Code ?? "N/A",
                CourseTitle = exam.Course?.Title ?? exam.EventName,
                Location = exam.Location ?? "غير محدد",
                TargetLevel = "الفرقة الأولى"
            };
        }
    }
}