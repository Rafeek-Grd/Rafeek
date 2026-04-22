using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentsInSection
{
    public class GetStudentInSectionQueryPagginatedValidator: AbstractValidator<GetStudentsInSectionQueryPagginated>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetStudentInSectionQueryPagginatedValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Course.SectionNotFound.Value])
                .MustAsync(SectionExists).WithMessage(_localizer[LocalizationKeys.Course.SectionNotFound.Value]);
        }

        private async Task<bool> SectionExists(Guid sectionId, CancellationToken cancellationToken)
        {
            return await _ctx.SectionRepository.ExistsAsync(x => x.Id == sectionId, cancellationToken);
        }
    }
}
