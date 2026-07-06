using MediatR;
using Rafeek.Application.Handlers.AcademicSchedules.DTOs;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.CreateAcadmicSchedule
{
    public class CreateAcadmicScheduleCommand : IRequest<AcademicScheduleDto>
    {
        public Guid CourseId { get; set; }
        public Guid? DoctorId { get; set; }
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int Capacity { get; set; }
        public string? Location { get; set; }
    }
}
