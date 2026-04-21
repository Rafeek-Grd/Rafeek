using MediatR;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamsSchedule
{
    public class GetExamsScheduleQuery : IRequest<List<ExamDayGroupDto>>
    {
        public string? TermId { get; set; }
        public string? SearchText { get; set; }
    }

    public class ExamDayGroupDto
    {
        public DateTime Date { get; set; }
        public string FormattedDate { get; set; } = null!;
        public string ExamCountLabel { get; set; } = null!;
        public List<ExamItemDto> Exams { get; set; } = new();
    }

    public class ExamItemDto
    {
        public Guid ExamId { get; set; }
        public string TimeLabel { get; set; } = null!;
        public string DurationLabel { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string TargetLevel { get; set; } = null!; // e.g. Undergrad Year 1
    }
}
