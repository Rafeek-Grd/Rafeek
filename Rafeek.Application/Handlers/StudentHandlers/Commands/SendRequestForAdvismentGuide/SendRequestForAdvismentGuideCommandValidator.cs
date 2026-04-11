using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.SendRequestForAdvismentGuide
{
    public class SendRequestForAdvismentGuideCommandValidator: AbstractValidator<SendRequestForAdvismentGuideCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IUnitOfWork _ctx;

        public SendRequestForAdvismentGuideCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Student.StudentIdRequired.Value])
                .MustAsync(StudentExists).WithMessage(_localizer[LocalizationKeys.Student.StudentsNotFound.Value]);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.StudentSupport.TitleRequired.Value]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.StudentSupport.DescriptionRequired.Value]);
        }

        private async Task<bool> StudentExists(Guid studentId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository.ExistsAsync(x => x.UserId == studentId, cancellationToken);
        }
    }
}
