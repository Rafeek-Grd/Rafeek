using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.EnrollStudent
{
    public class EnrollStudentCommandValidator: AbstractValidator<EnrollStudentCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public EnrollStudentCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.CourseId)
                .NotEmpty()
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.Course.NotFound.Value]);

            RuleFor(x => x.SectionId)
                .NotEmpty()
                .MustAsync(SectionExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }

        private async Task<bool> CourseExists(Guid courseId, CancellationToken cancellationToken)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
        }

        private async Task<bool> SectionExists(Guid sectionId, CancellationToken cancellationToken)
        {
            return await _ctx.SectionRepository.ExistsAsync(s => s.Id == sectionId, cancellationToken);
        }
    }
}
