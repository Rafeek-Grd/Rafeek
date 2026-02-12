using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class GPASimulatorLog : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public float ExpectedGPA { get; set; }
        public float PredictedCGPA { get; set; }
    }
}
