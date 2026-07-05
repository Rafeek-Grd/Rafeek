using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.DocumentHandlers.Queries.GetDocumentRequestsPaginated
{
    public class GetDocumentRequestsPaginatedQueryValidator : AbstractValidator<GetDocumentRequestsPaginatedQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetDocumentRequestsPaginatedQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.AdvisorId)
                .CustomAsync(async (advisorId, context, cancellationToken) =>
                {
                    if (advisorId.HasValue && !await AdvisorExists(advisorId.Value, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Advisor.AdvisorIdNotFound.Value]);
                    }
                })
                .When(x => x.AdvisorId.HasValue);

            RuleFor(x => x.DepartmentId)
                .CustomAsync(async (departmentId, context, cancellationToken) =>
                {
                    if (departmentId.HasValue && !await DepartmentExists(departmentId.Value, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Department.DepartmentIdNotFound.Value]);
                    }
                })
                .When(x => x.DepartmentId.HasValue);

            RuleFor(x => x.StudentId)
                .CustomAsync(async (studentId, context, cancellationToken) =>
                {
                    if (studentId.HasValue && !await StudentExists(studentId.Value, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Student.StudentsNotFound.Value]);
                    }
                })
                .When(x => x.StudentId.HasValue);
        }

        private async Task<bool> AdvisorExists(Guid advisorId, CancellationToken cancellationToken)
        {
            return await _ctx.DoctorRepository
                .ExistsAsync(x => x.Id == advisorId, cancellationToken);
        }

        private async Task<bool> DepartmentExists(Guid departmentId, CancellationToken cancellationToken)
        {
            return await _ctx.DepartmentRepository
                .ExistsAsync(x => x.Id == departmentId, cancellationToken);
        }

        private async Task<bool> StudentExists(Guid studentId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository
                .ExistsAsync(x => x.Id == studentId, cancellationToken);
        }
    }
}
