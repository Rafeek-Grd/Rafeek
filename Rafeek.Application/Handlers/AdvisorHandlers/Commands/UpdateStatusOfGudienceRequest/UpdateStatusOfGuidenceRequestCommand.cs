using MediatR;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands.UpdateStatusOfGudienceRequest
{
    public class UpdateStatusOfGuidenceRequestCommand: IRequest<Unit>
    {
        public Guid AdvisorId { get; set; }
        public Guid RequestId { get; set; }
        public StudentSupportStatus Status { get; set; }
    }
}
