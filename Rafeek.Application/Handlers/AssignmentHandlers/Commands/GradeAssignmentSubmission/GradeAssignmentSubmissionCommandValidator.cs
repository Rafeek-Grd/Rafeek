using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Commands.GradeAssignmentSubmission
{
    public class GradeAssignmentSubmissionCommandValidator : AbstractValidator<GradeAssignmentSubmissionCommand>
    {
        private readonly IUnitOfWork _ctx;

        public GradeAssignmentSubmissionCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.SubmissionId)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Assignment.SubmissionIdRequired.Value])
                .MustAsync(BeAValidSubmissionId).WithMessage(localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0).WithMessage(localizer[LocalizationKeys.Assignment.ScoreInvalid.Value]);
        }

        private async Task<bool> BeAValidSubmissionId(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _ctx.AssignmentSubmissionRepository.ExistsAsync(x => x.Id == submissionId, cancellationToken);
        }
    }
}
