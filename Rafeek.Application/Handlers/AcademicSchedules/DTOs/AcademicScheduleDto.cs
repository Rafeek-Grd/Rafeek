namespace Rafeek.Application.Handlers.AcademicSchedules.DTOs
{
    public class AcademicScheduleDto
    {
        public Guid LectureGroupId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public string Room { get; set; } = null!;
        public string? Location { get; set; }
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
