using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.CreateNewCourse
{
    public class CreateNewCourseCommandValidator : AbstractValidator<CreateNewCourseCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IUnitOfWork _ctx;

        public CreateNewCourseCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _localizer = localizer;
            _ctx = ctx;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localizer["Course_CodeRequired"])
                .MaximumLength(50).WithMessage(_localizer["Course_CodeMaxLength"]);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_localizer["Course_TitleRequired"])
                .MaximumLength(200).WithMessage(_localizer["Course_TitleMaxLength"]);

            RuleFor(x => x.CreditHours)
                .NotEmpty().WithMessage(_localizer["Course_CreditHoursRequired"])
                .InclusiveBetween(1, 10).WithMessage(_localizer["Course_CreditHoursInvalid"]);

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Department.DepartmentIdRequired.Value])
                .MustAsync((id, cancellation) => IsValidDepartmentIdAsync(id, cancellation));
        }

        private async Task<bool> IsValidDepartmentIdAsync(Guid? departmentId, CancellationToken cancellationToken)
        {
            return await _ctx.DepartmentRepository.ExistsAsync(d => d.Id == departmentId.Value, cancellationToken);
        }
    }
}
