namespace Rafeek.Application.Handlers.ExamSchedules.DTOs
{
    public class ExamDayGroupDto
    {
        public DateTime Date { get; set; }
        public string FormattedDate { get; set; } = null!;
        public string ExamCountLabel { get; set; } = null!;
        public List<ExamItemDto> Exams { get; set; } = new();
    }
}
