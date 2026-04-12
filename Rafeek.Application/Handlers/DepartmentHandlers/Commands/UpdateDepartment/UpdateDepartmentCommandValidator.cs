using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandValidator: AbstractValidator<UpdateDepartmentCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateDepartmentCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .MustAsync(IsDepartmentExists)
                .WithMessage(_localizer[LocalizationKeys.Department.DepartmentIdNotFound.Value]);
        }

        private async Task<bool> IsDepartmentExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.DepartmentRepository.ExistsAsync(x => x.Id == id);
        }
    }
}
