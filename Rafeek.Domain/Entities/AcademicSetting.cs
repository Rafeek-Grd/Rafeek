using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AcademicSetting : BaseEntity
    {
        public int MaxHoursPerSemester { get; set; }
        public int CourseCreditHours { get; set; }
        public bool AllowOverload { get; set; }
        public bool IncludeTransferHours { get; set; }
    }
}
