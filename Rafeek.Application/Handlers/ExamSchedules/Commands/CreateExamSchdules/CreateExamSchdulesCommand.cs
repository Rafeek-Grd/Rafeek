using MediatR;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.CreateExamSchdules
{
    public class CreateExamSchdulesCommand : IRequest<string>
    {
        public Guid CourseId { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public Guid? AcademicTermId { get; set; }
        public Guid? LectureGroupId { get; set; }
        public CalendarEventStatus Status { get; set; } = CalendarEventStatus.Draft;
    }
}