using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteCourseFromDepartment
{
    public class DeleteCourseFromDepartmentCommandHandler : IRequestHandler<DeleteCourseFromDepartmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteCourseFromDepartmentCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<Messages> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(DeleteCourseFromDepartmentCommand request, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.CourseRepository.GetAll(c => c.Id == request.CourseId).FirstOrDefaultAsync(cancellationToken);
            
            course!.DepartmentId = null;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _localizer[LocalizationKeys.Department.CourseRemovedSuccessfully.Value];
        }
    }
}
