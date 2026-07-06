namespace Rafeek.Application.Handlers.ExamSchedules.DTOs
{
    public class ExamItemDto
    {
        public Guid ExamId { get; set; }
        public string TimeLabel { get; set; } = null!;
        public string DurationLabel { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string TargetLevel { get; set; } = null!;
    }
}
