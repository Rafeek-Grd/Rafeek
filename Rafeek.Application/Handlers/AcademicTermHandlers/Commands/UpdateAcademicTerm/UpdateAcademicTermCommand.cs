using MediatR;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.UpdateAcademicTerm
{
    public class UpdateAcademicTermCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = null!;
        public TermType? TermType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? DropDeadline { get; set; }
        public DateTime? ExamStartDate { get; set; }
        public DateTime? ExamEndDate { get; set; }
        public Guid? AcademicYearId { get; set; }
    }
}
