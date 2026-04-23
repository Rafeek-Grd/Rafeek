using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Commands.SubmitSectionGrades
{
    public class SubmitSectionGradesCommandValidator : AbstractValidator<SubmitSectionGradesCommand>
    {
        private readonly IUnitOfWork _ctx;

        public SubmitSectionGradesCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Course.SectionIdRequired.Value])
                .MustAsync(SectionExists).WithMessage(localizer[LocalizationKeys.Course.SectionNotFound.Value]);

            RuleFor(x => x.Grades)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.GlobalValidationMessages.AddedFailed.Value]);
        }

        private async Task<bool> SectionExists(Guid sectionId, CancellationToken cancellationToken)
        {
            return await _ctx.SectionRepository.ExistsAsync(s => s.Id == sectionId, cancellationToken);
        }
    }
}
