using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class UserCalendarPreference : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public bool ShowAcademicEvents { get; set; } = true;
        public bool ShowGuidanceEvents { get; set; } = true;
        public bool ShowDeadlines { get; set; } = true;
        public bool ShowOfficialHolidays { get; set; } = false;
    }
}
