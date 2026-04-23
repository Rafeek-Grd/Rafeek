using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Commands.CreateAssignment
{
    public class CreateAssignmentCommandValidator : AbstractValidator<CreateAssignmentCommand>
    {
        private readonly IUnitOfWork _ctx;

        public CreateAssignmentCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Assignment.TitleRequired.Value])
                .MaximumLength(200).WithMessage(localizer[LocalizationKeys.Assignment.TitleMaxLength.Value]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Assignment.DescriptionRequired.Value]);

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Assignment.DueDateRequired.Value])
                .GreaterThan(DateTime.UtcNow).WithMessage(localizer[LocalizationKeys.Assignment.DueDateMustBeFuture.Value]);

            RuleFor(x => x.TotalScore)
                .GreaterThan(0).WithMessage(localizer[LocalizationKeys.Assignment.TotalScoreInvalid.Value]);

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Course.SectionIdRequired.Value])
                .MustAsync(SectionExists).WithMessage(localizer[LocalizationKeys.Course.SectionNotFound.Value]);
        }

        private async Task<bool> SectionExists(Guid sectionId, CancellationToken cancellationToken)
        {
            return await _ctx.SectionRepository.ExistsAsync(s => s.Id == sectionId, cancellationToken);
        }
    }
}
