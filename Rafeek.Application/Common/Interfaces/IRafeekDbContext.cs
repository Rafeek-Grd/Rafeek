using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        DbSet<Doctor> Doctors { get; }
        DbSet<DocumentRequest> DocumentRequests { get; }
        DbSet<Appointment> Appointments { get; }
        DbSet<StudentSupport> StudentSupports { get; }
        DbSet<AcademicCalendar> AcademicCalendars { get; }
        DbSet<CampusMapLocation> CampusMapLocations { get; }
        DbSet<Notification> Notifications { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IExecutionStrategy CreateExecutionStrategy();
    }
}
