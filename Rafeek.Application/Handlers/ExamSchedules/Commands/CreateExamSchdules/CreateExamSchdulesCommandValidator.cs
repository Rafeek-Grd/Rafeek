using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.CreateExamSchdules
{
    public class CreateExamSchdulesCommandValidator : AbstractValidator<CreateExamSchdulesCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateExamSchdulesCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Course.IdRequired.Value]);

            RuleFor(x => x.EventDate)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventDateRequired.Value]);

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.StartTimeRequired.Value]);

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeRequired.Value])
                .GreaterThan(x => x.StartTime).WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeMustBeGreaterThanStartTime.Value]);
        }
    }
}