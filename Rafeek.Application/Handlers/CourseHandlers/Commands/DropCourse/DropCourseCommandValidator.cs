using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.DropCourse
{
    public class DropCourseCommandValidator: AbstractValidator<DropCourseCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DropCourseCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages>  localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.CourseId)
                .NotEmpty()
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.Course.NotFound.Value]);
        }

        private async Task<bool> CourseExists(Guid courseId, CancellationToken cancellationToken)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
        }
    }
}
