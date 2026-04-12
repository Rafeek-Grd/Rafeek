using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AddDepartment
{
    public class AddDepartmentCommandValidator: AbstractValidator<AddDepartmentCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IUnitOfWork _ctx;

        public AddDepartmentCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _localizer = localizer;
            _ctx = ctx;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Department.DepartmentNameRequired.Value])
                .MaximumLength(100).WithMessage(_localizer[LocalizationKeys.Department.DepartmentNameMaxLength.Value]);
        
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Department.DepartmentCodeRequired.Value])
                .MaximumLength(10).WithMessage(_localizer[LocalizationKeys.Department.DepartmentCodeMaxLength.Value])
                .MustAsync(async (code, cancellation) =>
                {
                    var existingDepartment = await _ctx.DepartmentRepository.GetFirstAsync(x => x.Code == code);
                    return existingDepartment == null;
                }).WithMessage(_localizer[LocalizationKeys.Department.DepartmentCodeUnique.Value])
                .When(x => !string.IsNullOrEmpty(x.Code));
        }
    }
}
