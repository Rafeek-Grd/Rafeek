using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class AcademicYear : BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AcademicYearStatus Status { get; set; } = AcademicYearStatus.Draft;

        public ICollection<AcademicTerm> Terms { get; set; } = new HashSet<AcademicTerm>();
    }
}
