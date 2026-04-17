using System;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.StudentHandlers.DTOs
{
    public class StudentProfileDto
    {
        public string FullName { get; set; } = null!;
        public string UniversityCode { get; set; } = null!;
        public string? DepartmentName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public float CurrentGPA { get; set; }
        public float CumulativeGPA { get; set; }
        public int Level { get; set; }
        public int CompletedHours { get; set; }
        public int TotalHours { get; set; } = 135; // Default as per UI design
        public string? AcademicAdvisorName { get; set; }
        public List<AcademicHistoryDto> AcademicHistory { get; set; } = new();
    }

    public class AcademicHistoryDto
    {
        public string SemesterName { get; set; } = null!;
        public float SemesterGPA { get; set; }
        public List<CourseGradeDto> Courses { get; set; } = new();
    }

    public class CourseGradeDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public int CreditHours { get; set; }
        public string? Grade { get; set; } // Letter grade (A, B+)
        public float Score { get; set; } // Numeric score (90, 85)
    }
}
