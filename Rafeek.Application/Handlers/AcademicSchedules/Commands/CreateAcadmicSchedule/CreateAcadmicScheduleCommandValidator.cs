using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.CreateAcadmicSchedule
{
    public class CreateAcadmicScheduleCommandValidator : AbstractValidator<CreateAcadmicScheduleCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateAcadmicScheduleCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermIdRequired.Value])
                .MustAsync(CourseNotAlreadyAssigned).WithMessage(_localizer["CourseAlreadyAssigned"]);

            RuleFor(x => x.Day)
                .NotEmpty().WithMessage(_localizer["DayRequired"]);

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage(_localizer["TimeRequired"]);

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer["CapacityInvalid"]);
        }

        private async Task<bool> CourseNotAlreadyAssigned(Guid courseId, CancellationToken cancellationToken)
        {
            return !await _ctx.LectureGroupRepository.ExistsAsync(x => x.CourseId == courseId, cancellationToken);
        }
    }
}
