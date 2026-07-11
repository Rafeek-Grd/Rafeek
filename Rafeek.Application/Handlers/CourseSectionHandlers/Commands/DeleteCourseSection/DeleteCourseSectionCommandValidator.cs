using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.DeleteCourseSection
{
    public class DeleteCourseSectionCommandValidator : AbstractValidator<DeleteCourseSectionCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteCourseSectionCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .MustAsync(CourseSectionExists)
                .WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }

        private async Task<bool> CourseSectionExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.CourseSectionRepository.ExistsAsync(x => x.Id == id, cancellationToken);
        }
    }
}
