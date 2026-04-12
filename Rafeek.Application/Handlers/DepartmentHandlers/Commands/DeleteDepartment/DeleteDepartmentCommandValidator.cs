using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandValidator: AbstractValidator<DeleteDepartmentCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteDepartmentCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .MustAsync(IsDepartmentExists)
                .WithMessage(_localizer[LocalizationKeys.Department.DepartmentNotFound.Value]);
        }

        private async Task<bool> IsDepartmentExists(Guid id, CancellationToken cancellationToken)
        {
            return await _ctx.DepartmentRepository.ExistsAsync(d => d.Id == id, cancellationToken);
        }
    }
}
