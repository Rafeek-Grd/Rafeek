using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Enums;
using System.Globalization;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamsSchedule
{
    public class GetExamsScheduleQueryHandler : IRequestHandler<GetExamsScheduleQuery, List<ExamDayGroupDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetExamsScheduleQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamDayGroupDto>> Handle(GetExamsScheduleQuery request, CancellationToken cancellationToken)
        {
            var query = _context.AcademicCalendars
                .AsNoTracking()
                .Include(ac => ac.Course)
                .Where(ac => ac.EventType == AcademicCalendarEventType.Exam)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var lowerSearch = request.SearchText.ToLower();
                query = query.Where(ac =>
                    (ac.Course != null && (ac.Course.Title.ToLower().Contains(lowerSearch) || ac.Course.Code.ToLower().Contains(lowerSearch))) ||
                    ac.EventName.ToLower().Contains(lowerSearch));
            }

            var exams = await query
                .OrderBy(ac => ac.EventDate)
                .ThenBy(ac => ac.StartTime)
                .ToListAsync(cancellationToken);

            var grouped = exams
                .GroupBy(e => e.EventDate.Date)
                .Select(g =>
                {
                    string dayName = GetArabicDayName(g.Key.DayOfWeek);
                    string monthName = GetArabicMonthName(g.Key.Month);
                    string formattedDate = $"{dayName} {g.Key.Day} {monthName} {g.Key.Year}";

                    int count = g.Count();
                    string examCountLabel = count == 1 ? "1 امتحان" : $"{count} امتحانات";

                    return new ExamDayGroupDto
                    {
                        Date = g.Key,
                        FormattedDate = formattedDate,
                        ExamCountLabel = examCountLabel,
                        Exams = g.Select(e =>
                        {
                            var timeSpanStr = new DateTime(e.StartTime.Ticks).ToString("hh:mm tt", CultureInfo.InvariantCulture)
                                .Replace("AM", "صباحاً")
                                .Replace("PM", "مساءً")
                                .Replace("طهرًا", "ظهراً");
                                
                            var durationHours = (e.EndTime - e.StartTime).TotalHours;
                            string durationLabel = durationHours == 1 ? "1 ساعة" :
                                                   durationHours == 2 ? "ساعتان" :
                                                   durationHours > 2 && durationHours < 11 ? $"{durationHours} ساعات" :
                                                   $"{durationHours} ساعة";

                            return new ExamItemDto
                            {
                                ExamId = e.Id,
                                TimeLabel = timeSpanStr,
                                DurationLabel = durationLabel,
                                CourseCode = e.Course?.Code ?? "N/A",
                                CourseTitle = e.Course?.Title ?? e.EventName,
                                Location = e.Location ?? "غير محدد",
                                TargetLevel = "الفرقة الأولى"
                            };
                        }).ToList()
                    };
                })
                .ToList();

            return grouped;
        }

        private string GetArabicDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Saturday => "السبت",
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الاثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                DayOfWeek.Friday => "الجمعة",
                _ => ""
            };
        }

        private string GetArabicMonthName(int month)
        {
            return month switch
            {
                1 => "يناير",
                2 => "فبراير",
                3 => "مارس",
                4 => "أبريل",
                5 => "مايو",
                6 => "يونيو",
                7 => "يوليو",
                8 => "أغسطس",
                9 => "سبتمبر",
                10 => "أكتوبر",
                11 => "نوفمبر",
                12 => "ديسمبر",
                _ => ""
            };
        }
    }
}
