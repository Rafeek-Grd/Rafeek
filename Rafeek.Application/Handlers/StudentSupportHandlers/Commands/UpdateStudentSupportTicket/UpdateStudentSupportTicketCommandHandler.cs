using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.UpdateStudentSupportTicket
{
    public class UpdateStudentSupportTicketCommandHandler : IRequestHandler<UpdateStudentSupportTicketCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateStudentSupportTicketCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(UpdateStudentSupportTicketCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.StudentSupportRepository.FindByKeyAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            if (request.Title != null)
                entity.Title = request.Title;

            if (request.Description != null)
                entity.Description = request.Description;

            _ctx.StudentSupportRepository.Update(entity);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0
                ? _localizer[LocalizationKeys.GlobalValidationMessages.UpdatedSuccessfully.Value]
                : _localizer[LocalizationKeys.GlobalValidationMessages.UpdatedFailed.Value];
        }
    }
}
