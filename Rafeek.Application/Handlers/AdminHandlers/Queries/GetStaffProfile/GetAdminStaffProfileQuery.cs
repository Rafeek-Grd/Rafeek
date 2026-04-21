using MediatR;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStaffProfile
{
    public class GetAdminStaffProfileQuery : IRequest<AdminStaffProfileDto>
    {
        public Guid UserId { get; set; }
    }
}
