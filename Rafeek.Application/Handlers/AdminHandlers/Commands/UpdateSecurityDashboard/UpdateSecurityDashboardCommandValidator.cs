using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AdminHandlers.Commands.UpdateSecurityDashboard
{
    public class UpdateSecurityDashboardCommandValidator : AbstractValidator<UpdateSecurityDashboardCommand>
    {
        public UpdateSecurityDashboardCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(v => v.SessionTimeoutMinutes)
                .GreaterThan(0)
                .When(v => v.SessionTimeoutMinutes.HasValue)
                .WithMessage(localizer[LocalizationKeys.GlobalValidationMessages.SessionTimeoutInvalid.Value]);
        }
    }
}
