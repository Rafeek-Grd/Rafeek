using System.Collections.Generic;

namespace Rafeek.Application.Common.Models.AI
{
    public class StudentAIGradesDto
    {
        public float GPA { get; set; }
        public IDictionary<string, float> CourseGrades { get; set; } = new Dictionary<string, float>();
    }
}
