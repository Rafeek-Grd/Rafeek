using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetDepartmentByIdOrCode
{
    public class GetDepartmentByIdOrCodeQueryValidator : AbstractValidator<GetDepartmentByIdOrCodeQuery>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDepartmentByIdOrCodeQueryValidator(IStringLocalizer<Messages> localizer, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.IdOrCode)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentCodeRequired.Value]);

            RuleFor(x => x.IdOrCode)
                .MustAsync(async (idOrCode, cancellationToken) =>
                {
                    Guid.TryParse(idOrCode, out Guid guidId);
                    return await _unitOfWork.DepartmentRepository.ExistsAsync(d => d.Id == guidId || d.Code == idOrCode, cancellationToken);
                })
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentNotFound.Value]);
        }
    }
}
