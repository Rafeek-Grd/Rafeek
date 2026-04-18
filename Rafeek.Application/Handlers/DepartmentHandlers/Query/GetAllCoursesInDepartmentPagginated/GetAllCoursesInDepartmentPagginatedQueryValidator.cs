using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllCoursesInDepartmentPagginated
{
    public class GetAllCoursesInDepartmentPagginatedQueryValidator : AbstractValidator<GetAllCoursesInDepartmentPagginatedQuery>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCoursesInDepartmentPagginatedQueryValidator(IStringLocalizer<Messages> localizer, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentIdRequired.Value])
                .MustAsync(async (deptId, cancellationToken) =>
                {
                    return await _unitOfWork.DepartmentRepository.ExistsAsync(d => d.Id == deptId, cancellationToken);
                })
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentNotFound.Value]);
        }
    }
}
