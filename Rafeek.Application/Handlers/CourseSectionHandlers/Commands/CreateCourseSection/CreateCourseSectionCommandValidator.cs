using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.CreateCourseSection
{
    public class CreateCourseSectionCommandValidator : AbstractValidator<CreateCourseSectionCommand>
    {
        public CreateCourseSectionCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(localizer["CourseSection_CourseIdRequired"]);

            RuleFor(x => x.Day)
                .InclusiveBetween(0, 6).WithMessage(localizer["CourseSection_DayInvalid"]);

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage(localizer["CourseSection_DurationInvalid"]);

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage(localizer["CourseSection_CapacityInvalid"]);

            RuleFor(x => x.AvailableSeats)
                .GreaterThanOrEqualTo(0).WithMessage(localizer["CourseSection_AvailableSeatsInvalid"]);
        }
    }
}
