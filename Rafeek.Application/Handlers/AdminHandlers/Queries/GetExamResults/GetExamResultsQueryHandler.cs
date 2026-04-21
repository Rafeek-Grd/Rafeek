using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamResults
{
    public class GetExamResultsQueryHandler : IRequestHandler<GetExamResultsQuery, List<ExamResultItemDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetExamResultsQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamResultItemDto>> Handle(GetExamResultsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .Include(e => e.Student)
                    .ThenInclude(s => s.Department)
                .Include(e => e.Course)
                .Include(e => e.Grades)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.TabName))
            {
                var tab = request.TabName.ToLower();

                if (tab.Contains("general"))
                {
                    query = query.Where(e => e.Student.Department == null || e.Student.Department.Name.Contains("عام") || e.Student.Department.Name.Contains("General"));
                }
                else
                {
                    query = query.Where(e => e.Student.Department != null && 
                                             e.Student.Department.Name.ToLower().Contains(tab));
                }
            }

            var enrollments = await query.ToListAsync(cancellationToken);

            var items = new List<ExamResultItemDto>();

            foreach (var enrollment in enrollments)
            {
                var gradeEntity = enrollment.Grades.OrderByDescending(g => g.AbsoluteScore).FirstOrDefault();
                
                if (gradeEntity != null || !string.IsNullOrWhiteSpace(enrollment.Grade))
                {
                    items.Add(new ExamResultItemDto
                    {
                        EnrollmentId = enrollment.Id,
                        StudentCode = enrollment.Student.UniversityCode,
                        StudentName = enrollment.Student.User.FullName,
                        StudentEmail = enrollment.Student.User.Email ?? "-",
                        CourseTitle = enrollment.Course.Title,
                        LetterGrade = enrollment.Grade ?? gradeEntity?.GradeValue.ToString() ?? "N/A",
                        Score = gradeEntity?.AbsoluteScore ?? 0,
                        IsPublished = enrollment.Status == "Published" || !string.IsNullOrEmpty(enrollment.Grade)
                    });
                }
            }

            return items;
        }
    }
}
