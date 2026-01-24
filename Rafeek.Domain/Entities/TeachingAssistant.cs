using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class TeachingAssistant : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Guid? SupervisorId { get; set; }
        public Doctor? Supervisor { get; set; }
    }
}
