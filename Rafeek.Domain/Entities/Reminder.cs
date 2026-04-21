using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Reminder : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Reminder()
        {
            IsCompleted = false;
        }
    }
}
