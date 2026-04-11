using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Queries.GetAllGuidenceSupportRequests
{
    public class GetAllGuidenceSupportRequestsPagginatedQueryValidator: AbstractValidator<GetAllGuidenceSupportRequestsPagginatedQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetAllGuidenceSupportRequestsPagginatedQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.AdvisorId)
                .CustomAsync(async (advisorId, context, cancellationToken) =>
                {
                    if(!await AdvisorExists(advisorId, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Advisor.AdvisorIdNotFound.Value]);
                    }
                })
                .When(x => x.AdvisorId.HasValue);

            RuleFor(x => x.DepartmentId)
                .CustomAsync(async (departmentId, context, cancellationToken) =>
                {
                    if (!await DepartmentExists(departmentId, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Department.DepartmentIdNotFound.Value]);
                    }
                })
                .When(x => x.DepartmentId.HasValue);

            RuleFor(x => x.StudentId)
                .CustomAsync(async (studentId, context, cancellationToken) =>
                {
                    if (!await StudentExists(studentId, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Student.StudentsNotFound.Value]);
                    }
                })
                .When(x => x.StudentId.HasValue);

        }

        private async Task<bool> AdvisorExists(Guid? advisorId, CancellationToken cancellationToken)
        {
            if (!advisorId.HasValue)
            {
                return false;
            }

            return await _ctx.DoctorRepository
                .ExistsAsync(x => x.UserId == advisorId.Value
                               && x.IsAcademicAdvisor == true, cancellationToken);
        } 

        private async Task<bool> DepartmentExists(Guid? departmentId, CancellationToken cancellationToken)
        {
            if (!departmentId.HasValue)
            {
                return false;
            }

            return await _ctx.DepartmentRepository.
                ExistsAsync(x => x.Id == departmentId.Value, cancellationToken);
        }

        private async Task<bool> StudentExists(Guid? studentId, CancellationToken cancellationToken)
        {
            if (!studentId.HasValue)
            {
                return false;
            }

            return await _ctx.StudentRepository
                .ExistsAsync(x => x.UserId == studentId.Value, cancellationToken);
        }
    }
}
