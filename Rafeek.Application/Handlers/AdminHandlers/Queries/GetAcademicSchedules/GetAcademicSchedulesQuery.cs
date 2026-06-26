using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetAcademicSchedules
{
    public class GetAcademicSchedulesQuery : IRequest<PagginatedResult<AcademicScheduleDto>>
    {
        public Guid? TermId { get; set; }
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }

    public class AcademicScheduleDto
    {
        public Guid LectureGroupId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string InstructorName { get; set; } = null!;
        public string Room { get; set; } = null!;
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
