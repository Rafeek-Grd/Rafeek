using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentProfile
{
    public class GetStudentProfileQueryHandler : IRequestHandler<GetStudentProfileQuery, StudentProfileDto>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IRafeekDbContext _dbContext;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;

        public GetStudentProfileQueryHandler(
            IIdentityUnitOfWork identityUnitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IStringLocalizer<Messages> localizer,
            IRafeekDbContext dbContext)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _dbContext = dbContext;
            _identityUnitOfWork = identityUnitOfWork;
        }

        public async Task<StudentProfileDto> Handle(GetStudentProfileQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.UserId != Guid.Empty ? request.UserId : _currentUserService.UserId;

            if (studentId == Guid.Empty)
            {
                throw new UnauthorizedException(_localizer[LocalizationKeys.ExceptionMessage.Unauthorized]);
            }

            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => (s.Id == studentId || s.UserId == studentId) && s.IsActive, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException(_localizer[LocalizationKeys.Student.StudentProfileNotFound]);
            }

            var user = await _identityUnitOfWork.ApplicationUserRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == student.UserId, cancellationToken);

            var department = student.DepartmentId.HasValue 
                ? await _dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == student.DepartmentId, cancellationToken) 
                : null;

            var advisor = student.AcademicAdvisorId.HasValue 
                ? await _dbContext.Doctors.AsNoTracking().FirstOrDefaultAsync(doc => doc.Id == student.AcademicAdvisorId, cancellationToken) 
                : null;

            var profile = await _dbContext.StudentAcademicProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == student.AcademicProfileId, cancellationToken);

            student.User = user!;
            student.Department = department;
            student.AcademicAdvisor = advisor;
            student.AcademicProfile = profile;

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
                var sectionIds = enrollments.Select(e => e.LectureGroupId).Distinct().ToList();

                var grades = await _dbContext.Grades.AsNoTracking().Where(g => enrollmentIds.Contains(g.EnrollmentId)).ToListAsync(cancellationToken);
                var courses = await _dbContext.Courses.AsNoTracking().Where(c => courseIds.Contains(c.Id)).ToListAsync(cancellationToken);
                var sections = await _dbContext.LectureGroups
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(s => sectionIds.Contains(s.Id))
                    .ToListAsync(cancellationToken);

                var calendarEvents = await _dbContext.AcademicCalendars.AsNoTracking()
                    .Where(ce => sectionIds.Contains(ce.LectureGroupId ?? Guid.Empty))
                    .ToListAsync(cancellationToken);
                
                var termIds = calendarEvents.Select(ce => ce.AcademicTermId).Where(id => id.HasValue).Select(id => id!.Value).Distinct().ToList();
                var terms = await _dbContext.AcademicTerms.AsNoTracking().Where(t => termIds.Contains(t.Id)).ToListAsync(cancellationToken);
                
                var yearIds = terms.Select(t => t.AcademicYearId).Distinct().ToList();
                var years = await _dbContext.AcademicYears.AsNoTracking().Where(y => yearIds.Contains(y.Id)).ToListAsync(cancellationToken);

                var coursesDict = courses.ToDictionary(c => c.Id);
                var sectionsDict = sections.ToDictionary(s => s.Id);
                var gradesByEnrollment = grades.GroupBy(g => g.EnrollmentId).ToDictionary(g => g.Key, g => g.ToList());
                var calendarBySection = calendarEvents.GroupBy(ce => ce.LectureGroupId).ToDictionary(g => g.Key ?? Guid.Empty, g => g.ToList());
                var termsDict = terms.ToDictionary(t => t.Id);
                var yearsDict = years.ToDictionary(y => y.Id);

                foreach (var enrollment in enrollments)
                {
                    enrollment.Course = coursesDict.GetValueOrDefault(enrollment.CourseId)!;
                    enrollment.LectureGroup = sectionsDict.GetValueOrDefault(enrollment.LectureGroupId)!;
                    enrollment.Grades = gradesByEnrollment.GetValueOrDefault(enrollment.Id) ?? new List<Grade>();
                    
                    if (enrollment.LectureGroup != null)
                    {
                        enrollment.LectureGroup.CalendarEvents = calendarBySection.GetValueOrDefault(enrollment.LectureGroupId) ?? new List<AcademicCalendar>();
                        foreach (var ce in enrollment.LectureGroup.CalendarEvents)
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

            var allGrades = student.Enrollments.SelectMany(e => e.Grades).ToList();
            if (allGrades.Any())
            {
                var latestGrade = allGrades.OrderByDescending(g => g.CreatedAt).First();
                result.CurrentGPA = latestGrade.TermGPA;
                result.CumulativeGPA = latestGrade.CGPA;
            }

            var history = new List<AcademicHistoryDto>();

            var enrollmentsByTerm = student.Enrollments
                .GroupBy(e => {
                    var term = e.LectureGroup?.CalendarEvents?
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
