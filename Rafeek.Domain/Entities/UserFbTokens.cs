using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class UserFbTokens : BaseEntity
    {
        public string FbToken { get; set; } = null!;
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public bool IsAndroidDevice { get; set; }
        public bool IsIosDevice { get; set; }
    }
}
