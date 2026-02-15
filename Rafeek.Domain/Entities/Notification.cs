using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class Notification : BaseEntity
    {
        // النوع: أكاديمي، مجدول، أو إعلان
        public NotificationType Type { get; set; }

        // العنوان والرسالة (مشتركة لجميع الأنواع)
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        // للإشعارات الخاصة بطالب محدد (Academic)
        public Guid? StudentId { get; set; }
        public Student? Student { get; set; }

        // للإشعارات العامة (Scheduled & Announcement)
        public string? TargetGroup { get; set; }

        // للإشعارات المجدولة (Scheduled)
        public DateTime? SendTime { get; set; }

        // حالة القراءة (للإشعارات الخاصة بطالب)
        public bool IsRead { get; set; }

        public Notification()
        {
            IsRead = false;
            Type = NotificationType.Academic;
        }
    }
}
