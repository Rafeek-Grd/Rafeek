using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedulesById
{
    public class GetAcademicSchedulesByIdQueryValidator : AbstractValidator<GetAcademicSchedulesByIdQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetAcademicSchedulesByIdQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.LectureId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermIdRequired.Value])
                .MustAsync(LectureExists).WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermNotFound.Value]);
        }

        private async Task<bool> LectureExists(Guid LectureId, CancellationToken cancellationToken)
        {
            return await _ctx.LectureGroupRepository.ExistsAsync(t => t.Id == LectureId, cancellationToken);
        }
    }
}
