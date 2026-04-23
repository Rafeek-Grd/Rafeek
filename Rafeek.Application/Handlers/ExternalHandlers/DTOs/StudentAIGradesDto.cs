using System.Collections.Generic;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class StudentAIGradesDto
    {
        public string StudentId { get; set; } = null!;
        public float GPA { get; set; }
        public IDictionary<string, float> CourseGrades { get; set; } = new Dictionary<string, float>();
    }
}
