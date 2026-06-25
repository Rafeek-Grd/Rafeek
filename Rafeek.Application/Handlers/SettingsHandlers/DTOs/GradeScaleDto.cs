using System;

namespace Rafeek.Application.Handlers.SettingsHandlers.DTOs
{
    public class GradeScaleDto
    {
        public Guid Id { get; set; }
        public string GradeLetter { get; set; } = null!;
        public double MinPercentage { get; set; }
        public double GpaPoints { get; set; }
        public string ArabicDescription { get; set; } = null!;
    }
}
