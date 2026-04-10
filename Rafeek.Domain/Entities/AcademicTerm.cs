using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class AcademicTerm : BaseEntity
    {
        public string Name { get; set; } = null!;
        public TermType TermType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? DropDeadline { get; set; }
        public DateTime? ExamStartDate { get; set; }
        public DateTime? ExamEndDate { get; set; }

        public Guid AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; } = null!;

        public ICollection<AcademicCalendar> CalendarEvents { get; set; } = new HashSet<AcademicCalendar>();
    }
}
