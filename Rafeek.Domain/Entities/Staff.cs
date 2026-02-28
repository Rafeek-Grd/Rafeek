using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Staff : BaseEntity
    {
        public string? EmployeeCode { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
