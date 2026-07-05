using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.DeleteStudentSupportTicket
{
    public class DeleteStudentSupportTicketCommandValidator : AbstractValidator<DeleteStudentSupportTicketCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteStudentSupportTicketCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            When(x => x.MessageSupportId.HasValue, () =>
            {
                RuleFor(x => x.MessageSupportId!.Value)
                    .MustAsync(StudentSupportExists)
                    .WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
            });
        }

        private async Task<bool> StudentSupportExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.StudentSupportRepository.ExistsAsync(x => x.Id == id, cancellationToken);
        }
    }
}
