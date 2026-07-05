using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetStudentSupportById
{
    public class GetStudentSupportByIdQueryValidator : AbstractValidator<GetStudentSupportByIdQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetStudentSupportByIdQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.StudentSupport.RequestIdRequired.Value])
                .MustAsync(TicketExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }

        private async Task<bool> TicketExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.StudentSupportRepository.ExistsAsync(x => x.Id == id, cancellationToken);
        }
    }
}
