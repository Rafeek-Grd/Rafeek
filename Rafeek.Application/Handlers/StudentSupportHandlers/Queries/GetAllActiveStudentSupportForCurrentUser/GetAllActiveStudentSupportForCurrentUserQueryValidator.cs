using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllActiveStudentSupportForCurrentUser
{
    public class GetAllActiveStudentSupportForCurrentUserQueryValidator : AbstractValidator<GetAllActiveStudentSupportForCurrentUserQuery>
    {
        public GetAllActiveStudentSupportForCurrentUserQueryValidator(IStringLocalizer<Messages> localizer)
        {
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage(localizer[LocalizationKeys.ExceptionMessage.BadRequest.Value]);
            });
        }
    }
}
