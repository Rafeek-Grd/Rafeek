using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class CareerSuggestion : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string CareerPath { get; set; } = null!;
        public string Justification { get; set; } = null!;
    }
}
