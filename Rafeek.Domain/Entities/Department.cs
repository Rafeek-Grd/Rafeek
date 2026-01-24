using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
        public ICollection<TeachingAssistant> TeachingAssistants { get; set; } = new HashSet<TeachingAssistant>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
