using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail
{
    public class GetCourseDetailQueryHandler : IRequestHandler<GetCourseDetailQuery, CourseDetailDto>
    {
        private readonly IRafeekDbContext _context;

        public GetCourseDetailQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<CourseDetailDto> Handle(GetCourseDetailQuery request, CancellationToken cancellationToken)
        {
            // ── Load course with all needed relations ─────────────────────────────
            var course = await _context.Courses
                .AsNoTracking()
                .Include(c => c.Department)
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.Prerequisite)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Section)
                        .ThenInclude(s => s.Instructor)
                            .ThenInclude(i => i.User)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Section)
                        .ThenInclude(s => s.CalendarEvents)
                            .ThenInclude(ev => ev.AcademicTerm)
                .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

            if (course is null)
                throw new NotFoundException($"المقرر بالمعرّف {request.CourseId} غير موجود.");

            // ── Registration stats ────────────────────────────────────────────────
            int enrolledCount = course.Enrollments.Count;
            int capacity      = course.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault();

            string regStatus, regStatusLabel;
            if (capacity == 0)
            {
                regStatus = "Closed"; regStatusLabel = "إلغاء التسجيل";
            }
            else if (enrolledCount >= capacity)
            {
                regStatus = "Full"; regStatusLabel = "مكتمل";
            }
            else
            {
                regStatus = "Open"; regStatusLabel = "متاح";
            }

            // ── Enrollment dates (from calendar events linked to this course) ─────
            var firstEvent = course.Enrollments
                .SelectMany(e => e.Section.CalendarEvents)
                .OrderBy(ev => ev.EventDate)
                .FirstOrDefault();

            var academicTermName = course.Enrollments
                .SelectMany(e => e.Section.CalendarEvents)
                .Where(ev => ev.AcademicTerm != null)
                .Select(ev => ev.AcademicTerm!.Name)
                .FirstOrDefault();

            // ── Prerequisites with per-student status ─────────────────────────────
            HashSet<Guid> completedCourseIds = new();
            HashSet<Guid> inProgressCourseIds = new();

            if (request.StudentId.HasValue)
            {
                completedCourseIds = (await _context.Enrollments
                    .AsNoTracking()
                    .Where(e => e.StudentId == request.StudentId && e.Grade != null)
                    .Select(e => e.CourseId)
                    .ToListAsync(cancellationToken))
                    .ToHashSet();

                inProgressCourseIds = (await _context.Enrollments
                    .AsNoTracking()
                    .Where(e => e.StudentId == request.StudentId && e.Grade == null)
                    .Select(e => e.CourseId)
                    .ToListAsync(cancellationToken))
                    .ToHashSet();
            }

            var prerequisites = course.Prerequisites.Select(p =>
            {
                string studentStatus, studentStatusLabel;

                if (!request.StudentId.HasValue)
                {
                    studentStatus = "NotMet"; studentStatusLabel = "غير مكتمل";
                }
                else if (completedCourseIds.Contains(p.PrerequisiteId))
                {
                    studentStatus = "Met"; studentStatusLabel = "مكتمل";
                }
                else if (inProgressCourseIds.Contains(p.PrerequisiteId))
                {
                    studentStatus = "InProgress"; studentStatusLabel = "جاري";
                }
                else
                {
                    studentStatus = "NotMet"; studentStatusLabel = "غير مكتمل";
                }

                return new PrerequisiteStatusDto
                {
                    CourseId           = p.PrerequisiteId,
                    Code               = p.Prerequisite.Code,
                    Title              = p.Prerequisite.Title,
                    StudentStatus      = studentStatus,
                    StudentStatusLabel = studentStatusLabel
                };
            }).ToList();

            // ── Instructors (distinct across sections) ────────────────────────────
            var instructors = course.Enrollments
                .Select(e => e.Section.Instructor)
                .DistinctBy(i => i.Id)
                .Select(i => new CourseInstructorDto
                {
                    InstructorId    = i.Id,
                    FullName        = i.User.FullName,
                    Email           = i.User.Email!,
                    ProfilePictureUrl = i.User.ProfilePictureUrl
                })
                .ToList();

            // ── Study Plan Distribution ───────────────────────────────────────────
            int totalHours = course.CreditHours > 0 ? course.CreditHours : 1;
            int uniHours   = (int)Math.Round(totalHours * 0.26);
            int majHours   = (int)Math.Round(totalHours * 0.43);
            int elective   = totalHours - uniHours - majHours;

            var distribution = new StudyPlanDistributionDto
            {
                UniversityRequirementHours       = uniHours,
                MajorRequirementHours            = majHours,
                ElectiveHours                    = elective,
                UniversityRequirementPercentage  = MathF.Round(uniHours  * 100f / totalHours, 1),
                MajorRequirementPercentage       = MathF.Round(majHours  * 100f / totalHours, 1),
                ElectivePercentage               = MathF.Round(elective  * 100f / totalHours, 1)
            };

            // ── Course Notifications ──────────────────────────────────────────────
            var notifications = await _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == null)            // system-wide course notifications
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .Select(n => new CourseNotificationDto
                {
                    NotificationId = n.Id,
                    Title          = n.Title,
                    Message        = n.Message,
                    CreatedAt      = n.CreatedAt
                })
                .ToListAsync(cancellationToken);

            // ── Assemble ──────────────────────────────────────────────────────────
            return new CourseDetailDto
            {
                CourseId                = course.Id,
                Code                    = course.Code,
                Title                   = course.Title,
                Description             = course.Description,
                CreditHours             = course.CreditHours,
                DepartmentId            = course.DepartmentId,
                DepartmentName          = course.Department?.Name,
                RegistrationStatus      = regStatus,
                RegistrationStatusLabel = regStatusLabel,
                EnrolledStudents        = enrolledCount,
                Capacity                = capacity,
                StartDate               = firstEvent?.EventDate,
                RegistrationOpenDate    = firstEvent?.EventDate,
                AcademicTerm            = academicTermName,
                IsTheoretical           = true,   // يُحدَّد لاحقاً من حقل نوع المقرر عند إضافته للـ Entity
                IsPractical             = false,
                TargetLevel             = 3,       // يُحدَّد لاحقاً
                Prerequisites           = prerequisites,
                StudyPlanDistribution   = distribution,
                Instructors             = instructors,
                Notifications           = notifications
            };
        }
    }
}
