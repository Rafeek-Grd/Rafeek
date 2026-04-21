using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourses
{
    public class GetCoursesQueryHandlerPagginated : IRequestHandler<GetCoursesQueryPagginated, PagginatedResult<CourseListItemDto>>
    {
        private readonly IUnitOfWork _ctx;

        public GetCoursesQueryHandlerPagginated(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagginatedResult<CourseListItemDto>> Handle(
            GetCoursesQueryPagginated request,
            CancellationToken cancellationToken)
        {
            var query = _ctx.CourseRepository.GetAll().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                query = query.Where(c => c.Code.Contains(term) || c.Title.Contains(term));
            }

            if (request.DepartmentId.HasValue)
                query = query.Where(c => c.DepartmentId == request.DepartmentId);

            if (request.CreditHours.HasValue)
                query = query.Where(c => c.CreditHours == request.CreditHours);

            if (request.Semester.HasValue)
            {
                query = query.Where(c =>
                    c.Enrollments.Any(e =>
                        e.Section.CalendarEvents.Any(ev =>
                            ev.AcademicTerm != null &&
                            (int)ev.AcademicTerm.TermType == request.Semester.Value)));
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                var status = request.Status.ToLower();
                if (status == "full")
                {
                    query = query.Where(c => 
                        c.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault() > 0 &&
                        c.Enrollments.Count >= c.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault());
                }
                else if (status == "closed")
                {
                    query = query.Where(c => 
                        c.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault() == 0);
                }
                else if (status == "open")
                {
                    query = query.Where(c => 
                        c.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault() > 0 &&
                        c.Enrollments.Count < c.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault());
                }
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var rawItems = await query
                .OrderBy(c => c.Code)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Title,
                    c.CreditHours,
                    c.DepartmentId,
                    DepartmentName = c.Department != null ? c.Department.Name : null,
                    EnrolledCount = c.Enrollments.Count,
                    Capacity = c.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault(),
                    InstructorName = c.Enrollments.Select(e => e.Section.Instructor.User.FullName).FirstOrDefault(),
                    PrerequisiteCodes = c.Prerequisites.Select(p => p.Prerequisite.Code).ToList(),
                    AcademicTermName = c.Enrollments
                        .SelectMany(e => e.Section.CalendarEvents)
                        .Where(ev => ev.AcademicTerm != null)
                        .Select(ev => ev.AcademicTerm!.Name)
                        .FirstOrDefault()
                })
                .PaginatedListAsync(request.PageNumber,request.PageSize);

            var items = rawItems.Items.Select(c =>
            {
                return new CourseListItemDto
                {
                    CourseId = c.Id,
                    Code = c.Code,
                    Title = c.Title,
                    CreditHours = c.CreditHours,
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.DepartmentName,
                    InstructorName = c.InstructorName,
                    EnrolledStudents = c.EnrolledCount,
                    Capacity = c.Capacity,
                    PrerequisiteCodes = c.PrerequisiteCodes,
                    AcademicTerm = c.AcademicTermName,
                    Status = c.Capacity == 0 ? "Closed" : (c.EnrolledCount >= c.Capacity ? "Full" : "Open"),
                    StatusLabel = c.Capacity == 0 ? "إلغاء التسجيل" : (c.EnrolledCount >= c.Capacity ? "مكتمل" : "متاح")
                };
            }).ToList();

            return PagginatedResult<CourseListItemDto>.Create(
                items.AsReadOnly(),
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
    }
}
