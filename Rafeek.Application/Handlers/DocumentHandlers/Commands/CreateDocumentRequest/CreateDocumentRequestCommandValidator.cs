using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.CreateDocumentRequest
{
    public class CreateDocumentRequestCommandValidator: AbstractValidator<CreateDocumentRequestCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateDocumentRequestCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Student.StudentIdRequired.Value])
                .MustAsync(StudentExists).WithMessage(_localizer[LocalizationKeys.Student.StudentsNotFound.Value]);
        }

        private async Task<bool> StudentExists(Guid studentId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository.ExistsAsync(s => s.Id == studentId, cancellationToken);
        }
    }
}
