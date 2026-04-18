using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteCourseFromDepartment
{
    public class DeleteCourseFromDepartmentCommandValidator : AbstractValidator<DeleteCourseFromDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCourseFromDepartmentCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.CourseId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Department.CourseNotFound.Value])
                .MustAsync(async (courseId, cancellationToken) =>
                {
                    return await _unitOfWork.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
                })
                .WithMessage(localizer[LocalizationKeys.Department.CourseNotFound.Value]);
        }
    }
}
