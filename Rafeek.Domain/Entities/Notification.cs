using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Notification()
        {
            IsRead = false;
        }
    }
}
