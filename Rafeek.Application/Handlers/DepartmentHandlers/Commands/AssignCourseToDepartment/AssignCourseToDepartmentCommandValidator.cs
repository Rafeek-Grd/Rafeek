using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignCourseToDepartment
{
    public class AssignCourseToDepartmentCommandValidator : AbstractValidator<AssignCourseToDepartmentCommand>
    {
        private readonly IUnitOfWork _ctx;

        public AssignCourseToDepartmentCommandValidator(IStringLocalizer<Messages> localizer, IUnitOfWork ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.CourseId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Department.CourseIdRequired.Value])
                .MustAsync(CourseExistsBefore)
                .WithMessage(localizer[LocalizationKeys.Department.CourseNotFound.Value]);

            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentIdRequired.Value])
                .MustAsync(DepartmentExistsBefore)
                .WithMessage(localizer[LocalizationKeys.Department.DepartmentNotFound.Value]);
        }

        private async Task<bool> CourseExistsBefore(Guid courseId, CancellationToken cancellation)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellation);
        }

        private Task<bool> DepartmentExistsBefore(Guid departmentId, CancellationToken cancellation)
        {
            return _ctx.DepartmentRepository.ExistsAsync(d => d.Id == departmentId, cancellation);
        }
    }
}
