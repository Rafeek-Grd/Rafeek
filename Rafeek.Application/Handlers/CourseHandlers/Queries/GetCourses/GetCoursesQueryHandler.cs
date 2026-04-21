using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, PagginatedResult<CourseListItemDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetCoursesQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<PagginatedResult<CourseListItemDto>> Handle(
            GetCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Courses
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
                .AsQueryable();

            // ── Filters ──────────────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                query = query.Where(c =>
                    c.Code.Contains(term) ||
                    c.Title.Contains(term));
            }

            if (request.DepartmentId.HasValue)
                query = query.Where(c => c.DepartmentId == request.DepartmentId);

            if (request.CreditHours.HasValue)
                query = query.Where(c => c.CreditHours == request.CreditHours);

            if (request.Semester.HasValue)
                query = query.Where(c =>
                    c.Enrollments.Any(e =>
                        e.Section.CalendarEvents.Any(ev =>
                            ev.AcademicTerm != null &&
                            (int)ev.AcademicTerm.TermType == request.Semester.Value)));

            // ── Count ────────────────────────────────────────────────────────────
            int totalCount = await query.CountAsync(cancellationToken);

            // ── Projection ───────────────────────────────────────────────────────
            var rawItems = await query
                .OrderBy(c => c.Code)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Title,
                    c.CreditHours,
                    c.DepartmentId,
                    DepartmentName = c.Department != null ? c.Department.Name : null,
                    EnrolledCount = c.Enrollments.Count,
                    Capacity = c.Enrollments
                        .Select(e => e.Section.Capacity)
                        .FirstOrDefault(),
                    InstructorName = c.Enrollments
                        .Select(e => e.Section.Instructor.User.FullName)
                        .FirstOrDefault(),
                    PrerequisiteCodes = c.Prerequisites
                        .Select(p => p.Prerequisite.Code)
                        .ToList(),
                    AcademicTermName = c.Enrollments
                        .SelectMany(e => e.Section.CalendarEvents)
                        .Where(ev => ev.AcademicTerm != null)
                        .Select(ev => ev.AcademicTerm!.Name)
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            // ── Status Mapping ───────────────────────────────────────────────────
            var items = rawItems.Select(c =>
            {
                string status, statusLabel;
                if (c.EnrolledCount >= c.Capacity && c.Capacity > 0)
                {
                    status = "Full"; statusLabel = "مكتمل";
                }
                else if (c.Capacity == 0)
                {
                    status = "Closed"; statusLabel = "إلغاء التسجيل";
                }
                else
                {
                    status = "Open"; statusLabel = "متاح";
                }

                // Apply status filter here (after projection to avoid EF translation issues)
                return new CourseListItemDto
                {
                    CourseId            = c.Id,
                    Code                = c.Code,
                    Title               = c.Title,
                    CreditHours         = c.CreditHours,
                    DepartmentId        = c.DepartmentId,
                    DepartmentName      = c.DepartmentName,
                    InstructorName      = c.InstructorName,
                    EnrolledStudents    = c.EnrolledCount,
                    Capacity            = c.Capacity,
                    Status              = status,
                    StatusLabel         = statusLabel,
                    PrerequisiteCodes   = c.PrerequisiteCodes,
                    AcademicTerm        = c.AcademicTermName
                };
            });

            // Apply status filter in memory
            if (!string.IsNullOrWhiteSpace(request.Status))
                items = items.Where(i => i.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase));

            return PagginatedResult<CourseListItemDto>.Create(
                items.ToList().AsReadOnly(),
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
    }
}
