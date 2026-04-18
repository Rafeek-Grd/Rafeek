using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteUserFromDepartment
{
    public class DeleteUserFromDepartmentCommandValidator : AbstractValidator<DeleteUserFromDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserFromDepartmentCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.FullNameRequired.Value]);

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

            RuleFor(x => x.UserId)
                .MustAsync(CheckIfUserInDeparment)
                .WithMessage(localizer[LocalizationKeys.Department.UserNotInDepartment.Value]);
        }

        private async Task<bool> CheckIfUserInDeparment(Guid userId, CancellationToken cancellationToken)
        {
            var isStudent = await _unitOfWork.StudentRepository.ExistsAsync(s => s.UserId == userId && s.DepartmentId != null, cancellationToken);
            if (isStudent) return true;
            var isDoctor = await _unitOfWork.DoctorRepository.ExistsAsync(d => d.UserId == userId && d.DepartmentId != null, cancellationToken);
            if (isDoctor) return true;
            var isInstructor = await _unitOfWork.InstructorRepository.ExistsAsync(i => i.UserId == userId && i.DepartmentId != null, cancellationToken);
            if (isInstructor) return true;
            return false;
        }
    }
}
