using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllUsersInDepartmentPagginated
{
    public class GetAllUsersInDepartmentPagginatedQueryValidator: AbstractValidator<GetAllUsersInDepartmentPagginatedQuery>
    {
        private readonly IUnitOfWork _ctx;

        public GetAllUsersInDepartmentPagginatedQueryValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Department.DepartmentIdRequired])
                .MustAsync(DepartmentExists).WithMessage(localizer[LocalizationKeys.Department.DepartmentNotFound]);
        }

        private async Task<bool> DepartmentExists(Guid departmentId, CancellationToken cancellationToken)
        {
            return await _ctx.DepartmentRepository.ExistsAsync(d => d.Id == departmentId, cancellationToken);
        }
    }
}
