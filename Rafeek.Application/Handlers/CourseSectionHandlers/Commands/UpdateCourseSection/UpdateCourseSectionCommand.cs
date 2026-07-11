using MediatR;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.UpdateCourseSection
{
    public class UpdateCourseSectionCommand : IRequest<CourseSectionDto>
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public int Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Duration { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
    }
}
