using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentHandlers.Query
{
    public class GetStudentProfileQueryValidator : AbstractValidator<GetStudentProfileQuery>
    {
        private readonly IUnitOfWork _ctx;

        public GetStudentProfileQueryValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;
            
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Student.StudentIdRequired.Value])
                .MustAsync(StudentExistsBefore)
                .WithMessage(localizer[LocalizationKeys.Student.StudentsNotFound.Value]);
        }

        private async Task<bool> StudentExistsBefore(Guid userId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository.ExistsAsync(s => s.UserId == userId, cancellationToken);
        }
    }
}
