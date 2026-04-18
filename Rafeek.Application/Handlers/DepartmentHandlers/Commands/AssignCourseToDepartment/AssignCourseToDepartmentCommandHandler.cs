using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignCourseToDepartment
{
    public class AssignCourseToDepartmentCommandHandler : IRequestHandler<AssignCourseToDepartmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Messages> _localizer;

        public AssignCourseToDepartmentCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<Messages> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(AssignCourseToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.CourseRepository.IncludeAll(c => c.Id == request.CourseId).FirstOrDefaultAsync(cancellationToken);

            course!.DepartmentId = request.DepartmentId;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _localizer[LocalizationKeys.Department.CourseAssignedSuccessfully.Value];
        }
    }
}
