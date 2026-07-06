using MediatR;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.UpdateAcadmicSchedule
{
    public class UpdateAcadmicScheduleCommand : IRequest<string>
    {
        public Guid LectureId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? DoctorId { get; set; }
        public string? Day { get; set; }
        public string? Time { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Capacity { get; set; }
        public string? Location { get; set; }
    }
}
