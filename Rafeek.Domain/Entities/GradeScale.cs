using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class GradeScale : BaseEntity
    {
        public string GradeLetter { get; set; } = null!;
        public double MinPercentage { get; set; }
        public double GpaPoints { get; set; }
        public string ArabicDescription { get; set; } = null!;
    }
}
