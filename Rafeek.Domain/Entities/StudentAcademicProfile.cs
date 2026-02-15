using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class StudentAcademicProfile: BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public float GPA { get; set; }
        public float CGPA { get; set; }
        public int CompletedCredits { get; set; }
        public int RemainingCredits { get; set; }
        public string Standing { get; set; } = null!;
    }
}
