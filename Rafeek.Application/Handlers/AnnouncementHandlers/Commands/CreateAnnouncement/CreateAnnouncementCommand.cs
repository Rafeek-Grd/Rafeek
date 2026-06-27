using MediatR;
using System;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommand : IRequest<Guid>
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int AudienceType { get; set; } // 0: AllStudents, 1: SpecificDepartments, 2: AcademicLevel
        public Guid? DepartmentId { get; set; }
        public int? TargetLevel { get; set; }
        public bool SendInApp { get; set; }
        public bool SendEmail { get; set; }
        public bool SendSMS { get; set; }
        public bool IsUrgent { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
