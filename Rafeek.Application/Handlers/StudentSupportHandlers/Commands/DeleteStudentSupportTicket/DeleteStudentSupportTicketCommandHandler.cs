using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.DeleteStudentSupportTicket
{
    public class DeleteStudentSupportTicketCommandHandler : IRequestHandler<DeleteStudentSupportTicketCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteStudentSupportTicketCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(DeleteStudentSupportTicketCommand request, CancellationToken cancellationToken)
        {
            if (request.MessageSupportId.HasValue)
            {
                var entity = await _ctx.StudentSupportRepository.FindByKeyAsync(request.MessageSupportId.Value, cancellationToken);

                entity.IsActive = false;
                entity.IsDeleted = true;
                _ctx.StudentSupportRepository.Update(entity);
            }
            else
            {
                var allEntities = await _ctx.StudentSupportRepository.GetAll().ToListAsync(cancellationToken);
                allEntities.ForEach(e =>
                {
                    e.IsActive = false;
                    e.IsDeleted = true;
                });
                _ctx.StudentSupportRepository.UpdateRange(allEntities);
            }

            var result = await _ctx.SaveChangesAsync(cancellationToken);
            return result > 0
                ? _localizer[LocalizationKeys.GlobalValidationMessages.DeletedSuccessfully.Value]
                : _localizer[LocalizationKeys.GlobalValidationMessages.DeletedFailed.Value];
        }
    }
}
