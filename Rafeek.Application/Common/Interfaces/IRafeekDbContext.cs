using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;
using System;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IRafeekDbContext : IAsyncDisposable
    {
        DbSet<Department> Departments { get; }
        DbSet<Student> Students { get; }
        DbSet<Instructor> Instructors { get; }
        DbSet<StudentAcademicProfile> StudentAcademicProfiles { get; }
        DbSet<Course> Courses { get; }
        DbSet<UserFbTokens> FbTokens { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<CoursePrerequisite> CoursePrerequisites { get; }
        DbSet<IdentityUser<Guid>> ApplicationUsers { get; }
        DbSet<Section> Sections { get; }
        DbSet<Enrollment> Enrollments { get; }
        DbSet<Grade> Grades { get; set; }
        DbSet<AICourseRecommendation> AICourseRecommendations { get; }
        DbSet<CareerSuggestion> CareerSuggestions { get; }
        DbSet<AcademicFeedback> AcademicFeedbacks { get; }
        DbSet<GPASimulatorLog> GPASimulatorLogs { get; }
        DbSet<ChatbotQuery> ChatbotQueries { get; }
        DbSet<LearningResource> LearningResources { get; }
        DbSet<StudyPlan> StudyPlans { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
