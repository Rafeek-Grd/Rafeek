using System.Collections.Generic;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class BatchStudentAIGradesDto
    {
        public string UniversityCode { get; set; } = null!;
        public float GPA { get; set; }
        public IDictionary<string, float> CourseGrades { get; set; } = new Dictionary<string, float>();
    }
}
