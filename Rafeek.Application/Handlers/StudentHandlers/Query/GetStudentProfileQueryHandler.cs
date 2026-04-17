using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.StudentHandlers.Query
{
    public class GetStudentProfileQueryHandler : IRequestHandler<GetStudentProfileQuery, StudentProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IRafeekDbContext _dbContext;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;

        public GetStudentProfileQueryHandler(
            IUnitOfWork unitOfWork,
            IIdentityUnitOfWork identityUnitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IStringLocalizer<Messages> localizer,
            IRafeekDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _dbContext = dbContext;
            _identityUnitOfWork = identityUnitOfWork;
        }

        public async Task<StudentProfileDto> Handle(GetStudentProfileQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.UserId != Guid.Empty ? request.UserId : _currentUserService.UserId;

            var student = await _unitOfWork.StudentRepository
                .GetAll(s => s.Id == studentId || s.UserId == studentId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            var userTask = _identityUnitOfWork.ApplicationUserRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(u => u.Id == student!.UserId, cancellationToken);
            var departmentTask = student!.DepartmentId.HasValue 
                ? _unitOfWork.DepartmentRepository.GetAll(d => d.Id == student.DepartmentId).AsNoTracking().FirstOrDefaultAsync(cancellationToken) 
                : Task.FromResult<Department?>(null);
            var advisorTask = student.AcademicAdvisorId.HasValue 
                ? _unitOfWork.DoctorRepository.GetAll(doc => doc.Id == student.AcademicAdvisorId).AsNoTracking().FirstOrDefaultAsync(cancellationToken) 
                : Task.FromResult<Doctor?>(null);
            var profileTask = _unitOfWork.StudentAcademicProfileRepository.GetAll(p => p.Id == student.AcademicProfileId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            await Task.WhenAll(userTask, departmentTask, advisorTask, profileTask);

            student.User = userTask.Result!;
            student.Department = departmentTask.Result;
            student.AcademicAdvisor = advisorTask.Result;
            student.AcademicProfile = profileTask.Result;

            // 2.1 Fetch Advisor's User data if advisor exists (Zero Joins manual aggregation)
            if (student.AcademicAdvisor != null)
            {
                var advisorUser = await _identityUnitOfWork.ApplicationUserRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == student.AcademicAdvisor.UserId, cancellationToken);
                student.AcademicAdvisor.User = advisorUser!;
            }

            var enrollments = await _dbContext.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == student.Id)
                .ToListAsync(cancellationToken);
            
            student.Enrollments = enrollments;

            if (enrollments.Any())
            {
                var enrollmentIds = enrollments.Select(e => e.Id).ToList();
                var courseIds = enrollments.Select(e => e.CourseId).Distinct().ToList();
                var sectionIds = enrollments.Select(e => e.SectionId).Distinct().ToList();

                var gradesTask = _dbContext.Grades.AsNoTracking().Where(g => enrollmentIds.Contains(g.EnrollmentId)).ToListAsync(cancellationToken);
                var coursesTask = _dbContext.Courses.AsNoTracking().Where(c => courseIds.Contains(c.Id)).ToListAsync(cancellationToken);
                var sectionsTask = _dbContext.Sections.AsNoTracking().Where(s => sectionIds.Contains(s.Id)).ToListAsync(cancellationToken);

                await Task.WhenAll(gradesTask, coursesTask, sectionsTask);

                var grades = gradesTask.Result;
                var courses = coursesTask.Result;
                var sections = sectionsTask.Result;

                var calendarEvents = await _dbContext.AcademicCalendars.AsNoTracking()
                    .Where(ce => sectionIds.Contains(ce.SectionId ?? Guid.Empty))
                    .ToListAsync(cancellationToken);
                
                var termIds = calendarEvents.Select(ce => ce.AcademicTermId).Where(id => id.HasValue).Select(id => id!.Value).Distinct().ToList();
                var terms = await _unitOfWork.AcademicTermRepository.GetAll(t => termIds.Contains(t.Id)).AsNoTracking().ToListAsync(cancellationToken);
                
                var yearIds = terms.Select(t => t.AcademicYearId).Distinct().ToList();
                var years = await _unitOfWork.AcademicYearRepository.GetAll(y => yearIds.Contains(y.Id)).AsNoTracking().ToListAsync(cancellationToken);

                var coursesDict = courses.ToDictionary(c => c.Id);
                var sectionsDict = sections.ToDictionary(s => s.Id);
                var gradesByEnrollment = grades.GroupBy(g => g.EnrollmentId).ToDictionary(g => g.Key, g => g.ToList());
                var calendarBySection = calendarEvents.GroupBy(ce => ce.SectionId).ToDictionary(g => g.Key, g => g.ToList());
                var termsDict = terms.ToDictionary(t => t.Id);
                var yearsDict = years.ToDictionary(y => y.Id);

                foreach (var enrollment in enrollments)
                {
                    enrollment.Course = coursesDict.GetValueOrDefault(enrollment.CourseId)!;
                    enrollment.Section = sectionsDict.GetValueOrDefault(enrollment.SectionId)!;
                    enrollment.Grades = gradesByEnrollment.GetValueOrDefault(enrollment.Id) ?? new List<Grade>();
                    
                    if (enrollment.Section != null)
                    {
                        enrollment.Section.CalendarEvents = calendarBySection.GetValueOrDefault(enrollment.SectionId) ?? new List<AcademicCalendar>();
                        foreach (var ce in enrollment.Section.CalendarEvents)
                        {
                            if (ce.AcademicTermId.HasValue)
                            {
                                ce.AcademicTerm = termsDict.GetValueOrDefault(ce.AcademicTermId.Value);
                                if (ce.AcademicTerm != null)
                                {
                                    ce.AcademicTerm.AcademicYear = yearsDict.GetValueOrDefault(ce.AcademicTerm.AcademicYearId)!;
                                }
                            }
                        }
                    }
                }
            }

            var result = _mapper.Map<StudentProfileDto>(student);

            var history = new List<AcademicHistoryDto>();

            var enrollmentsByTerm = student.Enrollments
                .GroupBy(e => {
                    var term = e.Section?.CalendarEvents?
                        .OrderBy(ce => ce.CreatedAt)
                        .Select(ce => ce.AcademicTerm)
                        .FirstOrDefault(t => t != null);
                    return term;
                })
                .Where(g => g.Key != null)
                .OrderByDescending(g => g.Key!.StartDate);

            foreach (var termGroup in enrollmentsByTerm)
            {
                var term = termGroup.Key!;
                var termGpa = termGroup
                    .SelectMany(e => e.Grades)
                    .OrderByDescending(g => g.CreatedAt)
                    .Select(g => g.TermGPA)
                    .FirstOrDefault();

                var termDto = new AcademicHistoryDto
                {
                    SemesterName = $"{term.Name} {term.AcademicYear?.Name}",
                    SemesterGPA = termGpa,
                    Courses = termGroup.Select(e => _mapper.Map<CourseGradeDto>(e)).ToList()
                };

                history.Add(termDto);
            }

            result.AcademicHistory = history;

            return result;
        }
    }
}
