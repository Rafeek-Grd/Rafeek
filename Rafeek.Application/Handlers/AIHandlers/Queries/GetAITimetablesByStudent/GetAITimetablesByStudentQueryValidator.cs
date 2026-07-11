using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAITimetablesByStudent
{
    public class GetAITimetablesByStudentQueryValidator : AbstractValidator<GetAITimetablesByStudentQuery>
    {
        public GetAITimetablesByStudentQueryValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }
    }
}
