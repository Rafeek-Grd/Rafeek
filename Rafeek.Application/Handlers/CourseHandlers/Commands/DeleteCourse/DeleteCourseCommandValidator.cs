using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.DeleteCourse
{
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteCourseCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _localizer = localizer;
            _ctx = ctx;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer["Course_IdRequired"])
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }

        private async Task<bool> CourseExists(Guid courseId, CancellationToken cancellationToken)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
        }
    }
}
