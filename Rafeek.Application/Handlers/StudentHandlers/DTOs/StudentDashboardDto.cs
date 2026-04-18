using System.Collections.Generic;

namespace Rafeek.Application.Handlers.StudentHandlers.DTOs
{
    public class StudentDashboardDto
    {
        public string FirstName { get; set; } = string.Empty;
        public float CGPA { get; set; }
        public int EarnedHours { get; set; }
        public List<TermGpaDto> GpaProgress { get; set; } = new List<TermGpaDto>();
        public PlanProgressDto PlanProgress { get; set; } = new PlanProgressDto();
    }

    public class TermGpaDto
    {
        public string TermName { get; set; } = string.Empty;
        public float Gpa { get; set; }
    }

    public class PlanProgressDto
    {
        public int CompletedCourses { get; set; }
        public int RemainingCourses { get; set; }
        public float UniversityRequirementsPercentage { get; set; }
        public float MajorRequirementsPercentage { get; set; }
        public float ElectiveRequirementsPercentage { get; set; }
    }
}
