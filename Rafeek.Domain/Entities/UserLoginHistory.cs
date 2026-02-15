using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class UserLoginHistory : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public string? IpAddress { get; set; }
    }
}
