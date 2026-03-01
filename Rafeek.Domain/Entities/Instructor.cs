using Microsoft.AspNetCore.Identity;
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Instructor : BaseEntity
    {
        public string? EmployeeCode { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
