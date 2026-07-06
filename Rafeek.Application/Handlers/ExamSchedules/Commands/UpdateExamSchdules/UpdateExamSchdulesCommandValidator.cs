using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.UpdateExamSchdules
{
    public class UpdateExamSchdulesCommandValidator : AbstractValidator<UpdateExamSchdulesCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateExamSchdulesCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventIdRequired.Value])
                .MustAsync(ExamExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .When(x => x.StartTime.HasValue && x.EndTime.HasValue)
                .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeMustBeGreaterThanStartTime.Value]);
        }

        private async Task<bool> ExamExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicCalendarRepository.ExistsAsync(e => e.Id == id, cancellationToken);
        }
    }
}