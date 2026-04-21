using Microsoft.EntityFrameworkCore.Storage;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRafeekDbContext _context;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ICurrentUserService _currentUserService;


        private RefreshTokenRepository? _refreshTokenRepository;
        private UserFbTokenRepository? _userFbTokenRepository;
        private AcademicCalendarRepository? _academicCalendarRepository;
        private AcademicYearRepository? _academicYearRepository;
        private AcademicTermRepository? _academicTermRepository;
        private StudentRepository? _studentRepository;
        private DoctorRepository? _doctorRepository;
        private StudentSupportRepository? _studentSupportRepository;
        private DepartmentRepository? _departmentRepository;
        private CourseRepository? _courseRepository;
        private InstructorRepository? _instructorRepository;
        private StudentAcademicProfileRepository? _studentAcademicProfileRepository;
        private AITimetableRepository? _aiTimetableRepository;
        private AITimetableItemRepository? _aiTimetableItemRepository;
        private ReminderRepository? _reminderRepository;
        private CareerSuggestionRepository? _careerSuggestionRepository;
        private StudyPlanRepository? _studyPlanRepository;
        private LearningResourceRepository? _learningResourceRepository;
        private GPASimulatorLogRepository? _gpaSimulatorLogRepository;
        private DocumentRequestRepository? _documentRequestRepository;
        private SectionRepository? _sectionRepository;
        private EnrollmentRepository? _enrollmentRepository;
        private NotificationRepository? _notificationRepository;

        public UnitOfWork(
            IRafeekDbContext context,
            IJwtTokenManager jwtTokenManager,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
        }


        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(_context, _jwtTokenManager, _currentUserService);
        public IUserFbTokenRepository UserFbTokenRepository => _userFbTokenRepository ??= new UserFbTokenRepository(_context);
        public IAcademicCalendarRepository AcademicCalendarRepository => _academicCalendarRepository ??= new AcademicCalendarRepository(_context);
        public IAcademicYearRepository AcademicYearRepository => _academicYearRepository ??= new AcademicYearRepository(_context);
        public IAcademicTermRepository AcademicTermRepository => _academicTermRepository ??= new AcademicTermRepository(_context);
        public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(_context);
        public IDoctorRepository DoctorRepository => _doctorRepository ??= new DoctorRepository(_context);
        public IStudentSupportRepository StudentSupportRepository => _studentSupportRepository ??= new StudentSupportRepository(_context);
        public IDepartmentRepository DepartmentRepository => _departmentRepository ??= new DepartmentRepository(_context);
        public ICourseRepository CourseRepository => _courseRepository ??= new CourseRepository(_context);
        public IInstructorRepository InstructorRepository => _instructorRepository ??= new InstructorRepository(_context);
        public IStudentAcademicProfileRepository StudentAcademicProfileRepository => _studentAcademicProfileRepository ??= new StudentAcademicProfileRepository(_context);
        public IAITimetableRepository AITimetableRepository => _aiTimetableRepository ??= new AITimetableRepository(_context);
        public IAITimetableItemRepository AITimetableItemRepository => _aiTimetableItemRepository ??= new AITimetableItemRepository(_context);
        public IReminderRepository ReminderRepository => _reminderRepository ??= new ReminderRepository(_context);
        public ICareerSuggestionRepository CareerSuggestionRepository => _careerSuggestionRepository ??= new CareerSuggestionRepository(_context);
        public IStudyPlanRepository StudyPlanRepository => _studyPlanRepository ??= new StudyPlanRepository(_context);
        public ILearningResourceRepository LearningResourceRepository => _learningResourceRepository ??= new LearningResourceRepository(_context);
        public IGPASimulatorLogRepository GPASimulatorLogRepository => _gpaSimulatorLogRepository ??= new GPASimulatorLogRepository(_context);
        public IDocumentRequestRepository DocumentRequestRepository => _documentRequestRepository ??= new DocumentRequestRepository(_context);
        public ISectionRepository SectionRepository => _sectionRepository ??= new SectionRepository(_context);
        public IEnrollmentRepository EnrollmentRepository => _enrollmentRepository ??= new EnrollmentRepository(_context);
        public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.BeginTransactionAsync(cancellationToken);
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _context.CreateExecutionStrategy();
        }


        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
