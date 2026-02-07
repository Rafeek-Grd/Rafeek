using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class UserFbTokens : BaseEntity
    {
        public string FbToken { get; set; } = null!;
        public Guid UserId { get; set; }
        public bool IsAndroidDevice { get; set; }
        public bool IsIosDevice { get; set; }
    }
}
