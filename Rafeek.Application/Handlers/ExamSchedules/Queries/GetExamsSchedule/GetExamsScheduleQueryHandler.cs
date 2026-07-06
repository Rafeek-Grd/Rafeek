using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ExamSchedules.DTOs;
using Rafeek.Domain.Enums;
using System.Globalization;

namespace Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsSchedule
{
    public class GetExamsScheduleQueryHandler : IRequestHandler<GetExamsScheduleQuery, PagginatedResult<ExamDayGroupDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetExamsScheduleQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<PagginatedResult<ExamDayGroupDto>> Handle(GetExamsScheduleQuery request, CancellationToken cancellationToken)
        {
            var query = _context.AcademicCalendars
                .AsNoTracking()
                .Include(ac => ac.Course)
                .Where(ac => ac.EventType == AcademicCalendarEventType.Exam);

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var lowerSearch = request.SearchText.ToLower();
                query = query.Where(ac =>
                    (ac.Course != null && (ac.Course.Title.ToLower().Contains(lowerSearch) || ac.Course.Code.ToLower().Contains(lowerSearch))) ||
                    ac.EventName.ToLower().Contains(lowerSearch));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            IQueryable<Domain.Entities.AcademicCalendar> paginatedQuery;

            if (request.PageNumber == -1)
            {
                paginatedQuery = query.OrderBy(ac => ac.EventDate).ThenBy(ac => ac.StartTime);
            }
            else
            {
                paginatedQuery = query
                    .OrderBy(ac => ac.EventDate).ThenBy(ac => ac.StartTime)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var exams = await paginatedQuery.ToListAsync(cancellationToken);

            var grouped = exams
                .GroupBy(e => e.EventDate.Date)
                .Select(g =>
                {
                    var dayName = GetArabicDayName(g.Key.DayOfWeek);
                    var monthName = GetArabicMonthName(g.Key.Month);
                    var formattedDate = $"{dayName} {g.Key.Day} {monthName} {g.Key.Year}";

                    var count = g.Count();
                    var examCountLabel = count == 1 ? "1 امتحان" : $"{count} امتحانات";

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
                            var durationLabel = durationHours == 1 ? "1 ساعة" :
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

            return new PagginatedResult<ExamDayGroupDto>(grouped, totalCount, request.PageNumber, request.PageSize);
        }

        private static string GetArabicDayName(DayOfWeek day)
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

        private static string GetArabicMonthName(int month)
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
