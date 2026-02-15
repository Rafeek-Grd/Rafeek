using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class Notification : BaseEntity
    {

        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public Guid? StudentId { get; set; }
        public Student? Student { get; set; }
        public string? TargetGroup { get; set; }
        public DateTime? SendTime { get; set; }
        public bool IsRead { get; set; }
        public Notification()
        {
            IsRead = false;
            Type = NotificationType.Academic;
        }
    }
}
