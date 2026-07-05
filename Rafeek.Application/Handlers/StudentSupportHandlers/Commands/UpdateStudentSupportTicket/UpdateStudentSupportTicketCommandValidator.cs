using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.UpdateStudentSupportTicket
{
    public class UpdateStudentSupportTicketCommandValidator : AbstractValidator<UpdateStudentSupportTicketCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateStudentSupportTicketCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.StudentSupport.RequestIdRequired.Value])
                .MustAsync(TicketExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage(_localizer[LocalizationKeys.StudentSupport.TitleRequired.Value]);
            });

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage(_localizer[LocalizationKeys.StudentSupport.DescriptionRequired.Value]);
            });
        }

        private async Task<bool> TicketExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.StudentSupportRepository.ExistsAsync(x => x.Id == id, cancellationToken);
        }
    }
}
