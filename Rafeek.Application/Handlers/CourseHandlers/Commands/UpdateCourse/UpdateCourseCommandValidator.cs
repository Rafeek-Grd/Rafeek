using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.UpdateCourse
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateCourseCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _localizer = localizer;
            _ctx = ctx;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer["Course_IdRequired"])
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

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

        private async Task<bool> CourseExists(Guid courseId, CancellationToken cancellationToken)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
        }
    }
}
