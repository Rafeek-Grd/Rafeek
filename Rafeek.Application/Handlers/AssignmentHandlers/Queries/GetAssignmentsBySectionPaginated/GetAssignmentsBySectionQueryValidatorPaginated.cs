using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Queries.GetAssignmentsBySection
{
    public class GetAssignmentsBySectionQueryValidatorPaginated : AbstractValidator<GetAssignmentsBySectionQueryPaginated>
    {
        private readonly IUnitOfWork _ctx;

        public GetAssignmentsBySectionQueryValidatorPaginated(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;

            RuleFor(x => x.LectureGroupId)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Course.SectionIdRequired.Value])
                .MustAsync(SectionExists).WithMessage(localizer[LocalizationKeys.Course.SectionNotFound.Value]);
        }

        private async Task<bool> SectionExists(Guid lectureGroupId, CancellationToken cancellationToken)
        {
            return await _ctx.LectureGroupRepository.ExistsAsync(s => s.Id == lectureGroupId, cancellationToken);
        }
    }
}
