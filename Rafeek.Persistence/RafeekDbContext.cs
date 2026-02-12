using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Common;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence
{
    public class RafeekDbContext : DbContext, IRafeekDbContext
    {
        private readonly ICurrentUserService? _currentUserService;

        public RafeekDbContext(DbContextOptions<RafeekDbContext> options) : base(options)
        {
        }

        public RafeekDbContext(DbContextOptions<RafeekDbContext> options
            , ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<IdentityUser<Guid>> ApplicationUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<StudentAcademicProfile> StudentAcademicProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserFbTokens> FbTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<AICourseRecommendation> AICourseRecommendations { get; set; }
        public DbSet<CareerSuggestion> CareerSuggestions { get; set; }
        public DbSet<AcademicFeedback> AcademicFeedbacks { get; set; }
        public DbSet<GPASimulatorLog> GPASimulatorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(RafeekDbContext).Assembly,
                type => type.Namespace != null && type.Namespace.EndsWith("Configurations.RafeekConfiguration"));

            builder.HasDefaultSchema("dbo");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
            {

                warnings.Ignore(CoreEventId.InvalidIncludePathError);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = !string.IsNullOrEmpty(entry.Entity.CreatedBy)
                            ? entry.Entity.CreatedBy
                            : (_currentUserService?.UserId.ToString() ?? "System");
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = _currentUserService?.UserId.ToString() ?? "System";
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedAt = DateTime.Now;
                        entry.Entity.DeletedBy = _currentUserService?.UserId.ToString() ?? "System";
                        entry.Entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
