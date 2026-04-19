using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetStudentGrades
{
    public class GetStudentGradesQueryValidator: AbstractValidator<GetStudentGradesQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetStudentGradesQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage(_localizer[LocalizationKeys.Student.StudentIdRequired])
                .MustAsync(StudentExists)
                .WithMessage(_localizer[LocalizationKeys.Student.StudentsNotFound]);

        }

        private async Task<bool> StudentExists(Guid studentId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository.ExistsAsync(s => s.Id == studentId, cancellationToken);
        }
    }
}
