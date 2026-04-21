using MediatR;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentProfile
{
    public class GetAdminStudentProfileQuery : IRequest<AdminStudentProfileDto>
    {
        public Guid StudentId { get; set; }
    }
}
