using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.CreateNewCourse
{
    public class CreateNewCourseCommandValidator : AbstractValidator<CreateNewCourseCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateNewCourseCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localizer["Course_CodeRequired"])
                .MaximumLength(50).WithMessage(_localizer["Course_CodeMaxLength"]);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_localizer["Course_TitleRequired"])
                .MaximumLength(200).WithMessage(_localizer["Course_TitleMaxLength"]);

            RuleFor(x => x.CreditHours)
                .NotEmpty().WithMessage(_localizer["Course_CreditHoursRequired"])
                .InclusiveBetween(1, 10).WithMessage(_localizer["Course_CreditHoursInvalid"]);
        }
    }
}
