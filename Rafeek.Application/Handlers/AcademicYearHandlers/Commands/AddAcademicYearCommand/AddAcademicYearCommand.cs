using MediatR;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.AddAcademicYearCommand
{
    public class AddAcademicYearCommand : IRequest<Unit>
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentYear { get; set; }
    }
}
