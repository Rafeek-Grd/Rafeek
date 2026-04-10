using System;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.DTOs
{
    public class AcademicYearDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentYear { get; set; }
    }
}
