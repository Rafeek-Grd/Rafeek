using MediatR;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.BookAcademicService
{
    public class BookAcademicServiceCommand : IRequest<Guid>
    {
        public AcademicServiceType ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan Time { get; set; }
        public string Notes { get; set; } = null!;
    }
}
