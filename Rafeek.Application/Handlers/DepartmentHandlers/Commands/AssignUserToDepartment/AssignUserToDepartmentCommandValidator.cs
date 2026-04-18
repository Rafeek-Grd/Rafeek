using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignUserToDepartment
{
    public class AssignUserToDepartmentCommandValidator : AbstractValidator<AssignUserToDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignUserToDepartmentCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.FullNameRequired.Value]);

            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentIdRequired.Value]);

            RuleFor(x => x.DepartmentId)
                .MustAsync(async (deptId, cancellationToken) =>
                {
                    return await _unitOfWork.DepartmentRepository.ExistsAsync(d => d.Id == deptId, cancellationToken);
                })
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentNotFound.Value]);

            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellationToken) =>
                {
                    var isStudent = await _unitOfWork.StudentRepository.ExistsAsync(s => s.UserId == userId, cancellationToken);
                    if (isStudent) return true;

                    var isDoctor = await _unitOfWork.DoctorRepository.ExistsAsync(d => d.UserId == userId, cancellationToken);
                    if (isDoctor) return true;

                    var isInstructor = await _unitOfWork.InstructorRepository.ExistsAsync(i => i.UserId == userId, cancellationToken);
                    return isInstructor;
                })
                .WithMessage(localizer[LocalizationKeys.Department.UserRoleNotFound.Value]);
        }
    }
}
