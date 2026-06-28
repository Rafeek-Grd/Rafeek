using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class SecuritySetting : BaseEntity
    {
        public int SessionTimeoutMinutes { get; set; } = 15;
        public bool IsForcePasswordChangeEnabled { get; set; } = false;
    }
}
