using MediatR;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetAcademicSchedules
{
    public class GetAcademicSchedulesQuery : IRequest<List<AcademicScheduleDto>>
    {
        public Guid? TermId { get; set; }
    }

    public class AcademicScheduleDto
    {
        public Guid SectionId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string InstructorName { get; set; } = null!;
        public string Room { get; set; } = null!;
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Status { get; set; } = null!; 
    }
}
