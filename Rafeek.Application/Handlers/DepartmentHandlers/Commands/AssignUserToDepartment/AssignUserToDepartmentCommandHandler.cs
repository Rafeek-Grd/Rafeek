using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignUserToDepartment
{
    public class AssignUserToDepartmentCommandHandler : IRequestHandler<AssignUserToDepartmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Messages> _localizer;

        public AssignUserToDepartmentCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<Messages> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(AssignUserToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var studentTask = _unitOfWork.StudentRepository.GetAll(s => s.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            var doctorTask = _unitOfWork.DoctorRepository.GetAll(d => d.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            await Task.WhenAll(studentTask, doctorTask);

            var student = studentTask.Result;
            var doctor = doctorTask.Result;

            if (student != null)
            {
                student.DepartmentId = request.DepartmentId;
            }
            
            if (doctor != null)
            {
                doctor.DepartmentId = request.DepartmentId;
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _localizer[LocalizationKeys.Department.UserAssignedSuccessfully.Value];
        }
    }
}
