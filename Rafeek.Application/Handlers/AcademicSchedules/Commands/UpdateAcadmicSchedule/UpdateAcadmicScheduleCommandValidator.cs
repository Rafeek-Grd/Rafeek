using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.UpdateAcadmicSchedule
{
    public class UpdateAcadmicScheduleCommandValidator : AbstractValidator<UpdateAcadmicScheduleCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateAcadmicScheduleCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.LectureId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermIdRequired.Value])
                .MustAsync(LectureExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            RuleFor(x => x.CourseId)
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value])
                .When(x => x.CourseId.HasValue);

            RuleFor(x => x.Day)
                .NotEmpty().WithMessage(_localizer["DayRequired"])
                .When(x => x.Day is not null);

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage(_localizer["TimeRequired"])
                .When(x => x.Time is not null);

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer["CapacityInvalid"])
                .When(x => x.Capacity.HasValue);
        }

        private async Task<bool> LectureExists(Guid lectureId, CancellationToken cancellationToken)
        {
            return await _ctx.LectureGroupRepository.ExistsAsync(x => x.Id == lectureId, cancellationToken);
        }

        private async Task<bool> CourseExists(Guid? courseId, CancellationToken cancellationToken)
        {
            if (!courseId.HasValue) return false;
            return await _ctx.CourseRepository.ExistsAsync(x => x.Id == courseId.Value, cancellationToken);
        }
    }
}
