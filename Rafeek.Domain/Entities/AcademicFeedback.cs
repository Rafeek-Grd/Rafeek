using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AcademicFeedback : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string Strength { get; set; } = null!;
        public string Weakness { get; set; } = null!;
        public string Recommendation { get; set; } = null!;
    }
}
