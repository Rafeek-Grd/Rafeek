using Microsoft.AspNetCore.Identity;
using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string UniversityCode { get; set; } = null!;
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public StudentStatus Status { get; set; }
        public Guid AcademicProfileId { get; set; }
        public StudentAcademicProfile? AcademicProfile { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public ICollection<UserFbTokens> UserFbTokens { get; set; } = new List<UserFbTokens>();
        public ICollection<ChatbotQuery> ChatbotQueries { get; set; } = new HashSet<ChatbotQuery>();
        public ICollection<StudyPlan> StudyPlans { get; set; } = new HashSet<StudyPlan>();
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public ICollection<AcademicFeedback> AcademicFeedbacks { get; set; } = new HashSet<AcademicFeedback>();
    }
}
