using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedules
{
    public class GetAcademicSchedulesQueryValidator : AbstractValidator<GetAcademicSchedulesQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetAcademicSchedulesQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            When(_ => _.TermId.HasValue, () =>
            {
                RuleFor(_ => _.TermId)
                    .MustAsync(async (termId, cancellationToken) => await TermExists(termId, cancellationToken))
                    .WithMessage(_ => _localizer[LocalizationKeys.AcademicTerm.AcademicTermNotFound.Value]);
            });
        }

        private async Task<bool> TermExists(Guid? termId, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicTermRepository.ExistsAsync(t => t.Id == termId, cancellationToken);
        }
    }
}
