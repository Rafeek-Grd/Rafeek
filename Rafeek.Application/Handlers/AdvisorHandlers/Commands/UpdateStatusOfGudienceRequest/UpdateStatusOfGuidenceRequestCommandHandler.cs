using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands.UpdateStatusOfGudienceRequest
{
    public class UpdateStatusOfGuidenceRequestCommandHandler : IRequestHandler<UpdateStatusOfGuidenceRequestCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public UpdateStatusOfGuidenceRequestCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }
        public async Task<Unit> Handle(UpdateStatusOfGuidenceRequestCommand request, CancellationToken cancellationToken)
        {
            var studentSupportRequest = await _ctx.StudentSupportRepository
                .GetFirstIncludingAll(x => x.Id == request.RequestId)
                .FirstOrDefaultAsync(cancellationToken);

            studentSupportRequest!.StudentSupportStatus = request.Status;
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
