using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands.AssignStudentsToAcademicAdvisor
{
    public class AssignStudentToAcademicAdvisorCommandValidator: AbstractValidator<AssignStudentsToAcademicAdvisorCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public AssignStudentToAcademicAdvisorCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x)
                .CustomAsync(async (command ,context, cancellationToken) =>
                {
                    if (!await UserExists(command.AcademicAdvisorId, cancellationToken))
                    {
                        context.AddFailure(nameof(command.AcademicAdvisorId), _localizer[LocalizationKeys.UserMessages.NotFound.Value]);
                    }

                    if(!await StudentsExists(command.StudentIds, cancellationToken))
                    {
                        context.AddFailure(nameof(command.StudentIds), _localizer[LocalizationKeys.Student.StudentsNotFound.Value]);
                    }
                });

        }

        private async Task<bool> UserExists(Guid academicAdvisorId, CancellationToken cancellationToken)
        {
            return await _ctx.DoctorRepository.ExistsAsync(x => x.UserId == academicAdvisorId && x.IsAcademicAdvisor == true, cancellationToken);
        }

        private async Task<bool> StudentsExists(List<Guid> studentIds, CancellationToken cancellationToken)
        {
            return await _ctx.StudentRepository.ExistsAsync(x => studentIds.Contains(x.UserId), cancellationToken);
        }
    }
}
