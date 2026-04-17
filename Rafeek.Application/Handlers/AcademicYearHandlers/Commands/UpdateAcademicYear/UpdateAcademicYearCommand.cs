using MediatR;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrentYear { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
