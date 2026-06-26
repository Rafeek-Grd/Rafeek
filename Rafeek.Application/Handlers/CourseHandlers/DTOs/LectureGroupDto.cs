using System;

namespace Rafeek.Application.Handlers.CourseHandlers.DTOs
{
    public class LectureGroupDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid? DoctorId { get; set; }
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int Capacity { get; set; }
    }
}
