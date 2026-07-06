using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsScheduleById
{
    public class GetExamsScheduleQueryValidator : AbstractValidator<GetExamsScheduleByIdQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetExamsScheduleQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventIdRequired.Value])
                .MustAsync(ExamExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }

        private async Task<bool> ExamExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicCalendarRepository.ExistsAsync(e => e.Id == id, cancellationToken);
        }
    }
}