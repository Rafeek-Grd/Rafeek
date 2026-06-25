using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetExamResults
{
    public class GetExamResultsQuery : IRequest<PagginatedResult<ExamResultItemDto>>
    {
        public string? TabName { get; set; }
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
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
