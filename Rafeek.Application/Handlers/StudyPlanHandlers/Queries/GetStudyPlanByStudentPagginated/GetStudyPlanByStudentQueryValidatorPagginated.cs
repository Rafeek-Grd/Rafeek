using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudyPlanHandlers.Queries.GetStudyPlanByStudent
{
    public class GetStudyPlanByStudentQueryValidatorPagginated: AbstractValidator<GetStudyPlanByStudentQueryPagginated>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetStudyPlanByStudentQueryValidatorPagginated(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Student.StudentIdRequired.Value])
                .MustAsync(StudentExists).WithMessage(_localizer[LocalizationKeys.Student.StudentsNotFound.Value]);
        }

        private async Task<bool> StudentExists(Guid studentId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository.ExistsAsync(x => x.Id == studentId, cancellationToken);
        }
    }
}
