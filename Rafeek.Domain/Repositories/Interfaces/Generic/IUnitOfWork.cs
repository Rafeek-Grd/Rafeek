using Microsoft.EntityFrameworkCore.Storage;

namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IUserFbTokenRepository UserFbTokenRepository { get; }
        IAcademicCalendarRepository AcademicCalendarRepository { get; }
        IAcademicYearRepository AcademicYearRepository { get; }
        IAcademicTermRepository AcademicTermRepository { get; }
        IStudentRepository StudentRepository { get; }
        IDoctorRepository DoctorRepository { get; }
        IStudentSupportRepository StudentSupportRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        ICourseRepository CourseRepository { get; }
        IStudentAcademicProfileRepository StudentAcademicProfileRepository { get; }
        IInstructorRepository InstructorRepository { get; }
        IAITimetableRepository AITimetableRepository { get; }
        IAITimetableItemRepository AITimetableItemRepository { get; }
        IReminderRepository ReminderRepository { get; }
        ICareerSuggestionRepository CareerSuggestionRepository { get; }
        IStudyPlanRepository StudyPlanRepository { get; }
        ILearningResourceRepository LearningResourceRepository { get; }
        IGPASimulatorLogRepository GPASimulatorLogRepository { get; }
        IDocumentRequestRepository DocumentRequestRepository { get; }
        ISectionRepository SectionRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IExecutionStrategy CreateExecutionStrategy();
    }
}
