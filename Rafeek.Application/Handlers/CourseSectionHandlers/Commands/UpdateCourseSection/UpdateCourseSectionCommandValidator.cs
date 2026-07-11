using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.UpdateCourseSection
{
    public class UpdateCourseSectionCommandValidator : AbstractValidator<UpdateCourseSectionCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateCourseSectionCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer["CourseSection_IdRequired"])
                .MustAsync(CourseSectionExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(_localizer["CourseSection_CourseIdRequired"]);

            RuleFor(x => x.Day)
                .InclusiveBetween(0, 6).WithMessage(_localizer["CourseSection_DayInvalid"]);

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage(_localizer["CourseSection_DurationInvalid"]);

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage(_localizer["CourseSection_CapacityInvalid"]);

            RuleFor(x => x.AvailableSeats)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer["CourseSection_AvailableSeatsInvalid"]);
        }

        private async Task<bool> CourseSectionExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.CourseSectionRepository.ExistsAsync(x => x.Id == id, cancellationToken);
        }
    }
}
