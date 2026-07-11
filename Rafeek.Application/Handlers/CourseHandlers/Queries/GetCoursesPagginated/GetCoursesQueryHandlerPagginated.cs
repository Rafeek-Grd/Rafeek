using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;
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
                        e.LectureGroup.CalendarEvents.Any(ev =>
                            ev.AcademicTerm != null &&
                            (int)ev.AcademicTerm.TermType == request.Semester.Value)));
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                var status = request.Status.ToLower();
                if (status == "full")
                {
                    query = query.Where(c => 
                        c.LectureGroups.Sum(lg => lg.Capacity) > 0 &&
                        c.Enrollments.Count >= c.LectureGroups.Sum(lg => lg.Capacity));
                }
                else if (status == "closed")
                {
                    query = query.Where(c => 
                        c.LectureGroups.Sum(lg => lg.Capacity) == 0);
                }
                else if (status == "open")
                {
                    query = query.Where(c => 
                        c.LectureGroups.Sum(lg => lg.Capacity) > 0 &&
                        c.Enrollments.Count < c.LectureGroups.Sum(lg => lg.Capacity));
                }
            }

            var result = await query
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
                    Capacity = c.LectureGroups.Sum(lg => lg.Capacity),
                    InstructorName = c.LectureGroups.Select(lg => lg.Doctor != null ? lg.Doctor.User.FullName : null).FirstOrDefault(),
                    PrerequisiteCodes = c.Prerequisites.Select(p => p.Prerequisite.Code).ToList(),
                     Sections = c.CourseSections.Select(s => new CourseSectionDto
                     {
                         Id = s.Id,
                         CourseId = s.CourseId,
                         Day = s.Day,
                         StartTime = s.StartTime,
                         Duration = s.Duration,
                         Capacity = s.Capacity,
                         AvailableSeats = s.AvailableSeats
                     }).ToList(),
                    AcademicTermName = c.Enrollments
                        .SelectMany(e => e.LectureGroup.CalendarEvents)
                        .Where(ev => ev.AcademicTerm != null)
                        .Select(ev => ev.AcademicTerm!.Name)
                        .FirstOrDefault()
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            var items = result.Items.Select(c => new CourseListItemDto
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
                Sections = c.Sections,
                Status = c.Capacity == 0 ? "Closed" : (c.EnrolledCount >= c.Capacity ? "Full" : "Open"),
                StatusLabel = c.Capacity == 0 ? "Ø¥Ù„ØºØ§Ø¡ Ø§Ù„ØªØ³Ø¬ÙŠÙ„" : (c.EnrolledCount >= c.Capacity ? "Ù…ÙƒØªÙ…Ù„" : "Ù…ØªØ§Ø­")
            }).ToList();

            return new PagginatedResult<CourseListItemDto>(
                items.AsReadOnly(),
                result.TotalCount,
                result.PageNumber,
                result.PageSize);
        }
    }
}


