using System.Collections.Generic;

namespace Rafeek.Application.Handlers.SettingsHandlers.DTOs
{
    public class AcademicSettingsDto
    {
        public int MaxHoursPerSemester { get; set; }
        public int CourseCreditHours { get; set; }
        public bool AllowOverload { get; set; }
        public bool IncludeTransferHours { get; set; }
        public List<GradeScaleDto> GradeScales { get; set; } = new();
    }
}
