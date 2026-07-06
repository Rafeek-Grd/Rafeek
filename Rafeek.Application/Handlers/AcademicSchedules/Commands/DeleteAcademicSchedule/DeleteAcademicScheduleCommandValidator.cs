using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.DeleteAcademicSchedule
{
    public class DeleteAcademicScheduleCommandValidator : AbstractValidator<DeleteAcademicScheduleCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteAcademicScheduleCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.LectureId)
                .MustAsync(LectureExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value])
                .When(x => x.LectureId.HasValue);
        }

        private async Task<bool> LectureExists(Guid? lectureId, CancellationToken cancellationToken)
        {
            if (!lectureId.HasValue) return false;
            return await _ctx.LectureGroupRepository.ExistsAsync(x => x.Id == lectureId.Value, cancellationToken);
        }
    }
}
