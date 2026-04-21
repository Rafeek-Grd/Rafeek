using MediatR;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamResults
{
    public class GetExamResultsQuery : IRequest<List<ExamResultItemDto>>
    {
        public string? TabName { get; set; } // e.g. "AI", "SWE", "1st General"
    }

    public class ExamResultItemDto
    {
        public Guid EnrollmentId { get; set; }
        public string StudentCode { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string LetterGrade { get; set; } = null!;
        public float Score { get; set; }
        public bool IsPublished { get; set; }
    }
}
