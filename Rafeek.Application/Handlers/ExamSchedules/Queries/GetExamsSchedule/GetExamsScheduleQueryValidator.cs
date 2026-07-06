using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsSchedule
{
    public class GetExamsScheduleQueryValidator: AbstractValidator<GetExamsScheduleQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetExamsScheduleQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
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
