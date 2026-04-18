using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteUserFromDepartment
{
    public class DeleteUserFromDepartmentCommandHandler : IRequestHandler<DeleteUserFromDepartmentCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteUserFromDepartmentCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<Messages> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteUserFromDepartmentCommand request, CancellationToken cancellationToken)
        {
            var studentTask = _unitOfWork.StudentRepository.GetAll(s => s.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            var doctorTask = _unitOfWork.DoctorRepository.GetAll(d => d.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            var instructorTask = _unitOfWork.InstructorRepository.GetAll(i => i.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);

            await Task.WhenAll(studentTask, doctorTask, instructorTask);

            var student = studentTask.Result;
            var doctor = doctorTask.Result;
            var instructor = instructorTask.Result;

            if (student != null)
            {
                student.DepartmentId = null;
            }

            if (doctor != null)
            {
                doctor.DepartmentId = null;
            }

            if (instructor != null)
            {
                instructor.DepartmentId = null;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.Ok(true, _localizer[LocalizationKeys.Department.UserRemovedSuccessfully.Value]);
        }
    }
}
