using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.DeleteExamSchdules
{
    public class DeleteExamSchdulesCommandValidator : AbstractValidator<DeleteExamSchdulesCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteExamSchdulesCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .MustAsync(ExamExists).WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value])
                .When(x => x.Id.HasValue);
        }

        private async Task<bool> ExamExists(Guid? id, CancellationToken cancellationToken)
        {
            if (!id.HasValue) return false;
            return await _ctx.AcademicCalendarRepository.ExistsAsync(e => e.Id == id.Value, cancellationToken);
        }
    }
}