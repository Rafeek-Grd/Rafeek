using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public RefreshTokenCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(v => v.Token)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(_localizer[LocalizationKeys.TokenMessages.Required.Value])
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.TokenMessages.NotValid.Value]);
        }
    }
}
