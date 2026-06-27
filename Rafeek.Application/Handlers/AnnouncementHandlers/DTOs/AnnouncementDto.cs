using System;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.DTOs
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int AudienceType { get; set; } // 0: AllStudents, 1: SpecificDepartments, 2: AcademicLevel
        public string AudienceTypeLabel { get; set; } = null!;
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? TargetLevel { get; set; }
        public bool SendInApp { get; set; }
        public bool SendEmail { get; set; }
        public bool SendSMS { get; set; }
        public bool IsUrgent { get; set; }
        public DateTime ScheduledAt { get; set; }
        public bool IsDeactivated { get; set; }
        public DateTime? PostponedTo { get; set; }
        public bool IsSent { get; set; }
        public string StatusLabel { get; set; } = null!;
    }
}
