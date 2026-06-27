using Rafeek.Domain.Common;
using System;

namespace Rafeek.Domain.Entities
{
    public class Announcement : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        
        // Target Audience
        public int AudienceType { get; set; } // 0: AllStudents, 1: SpecificDepartments, 2: AcademicLevel
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? TargetLevel { get; set; }

        // Delivery Channels
        public bool SendInApp { get; set; }
        public bool SendEmail { get; set; }
        public bool SendSMS { get; set; }

        // Settings
        public bool IsUrgent { get; set; }
        public DateTime ScheduledAt { get; set; }
        public bool IsDeactivated { get; set; }
        public DateTime? PostponedTo { get; set; }
        public bool IsSent { get; set; }
    }
}
