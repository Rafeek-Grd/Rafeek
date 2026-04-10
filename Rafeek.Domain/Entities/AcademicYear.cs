using Rafeek.Domain.Common;
using System;

namespace Rafeek.Domain.Entities
{
    public class AcademicYear : BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentYear { get; set; }

        public ICollection<AcademicTerm> Terms { get; set; } = new HashSet<AcademicTerm>();
    }
}
