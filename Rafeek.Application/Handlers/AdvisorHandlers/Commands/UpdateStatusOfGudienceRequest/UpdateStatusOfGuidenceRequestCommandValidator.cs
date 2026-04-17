using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands.UpdateStatusOfGudienceRequest
{
    public class UpdateStatusOfGuidenceRequestCommandValidator: AbstractValidator<UpdateStatusOfGuidenceRequestCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateStatusOfGuidenceRequestCommandValidator(IUnitOfWork ctx, ICurrentUserService currentUserService, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
            _localizer = localizer;

            RuleFor(x => x.RequestId)
                .CustomAsync(async (requestId, context, cancellationToken) =>
                {
                    if (string.IsNullOrEmpty(requestId.ToString()))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.StudentSupport.RequestIdRequired.Value]);
                        return;
                    }

                    if (!await IsValidRequest(requestId, cancellationToken) && !await CheckIfOnOwnedSupportRequest(requestId, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.Advisor.YouAreNotAdvisorOfThatStudent.Value]);
                    }
                });

            RuleFor(x => x.AdvisorId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Advisor.AdvisorIdRequired.Value])
                .MustAsync(IsValidAdvisorId).WithMessage(_localizer[LocalizationKeys.Advisor.AdvisorIdNotFound.Value]);
        }

        private async Task<bool> IsValidRequest(Guid requestId, CancellationToken cancellationToken)
        {
            return await _ctx.StudentSupportRepository
                .ExistsAsync(x => x.Id == requestId
                               && x.StudentSupportStatus == Domain.Enums.StudentSupportStatus.Pending, cancellationToken);
        }

        private Task<bool> IsValidAdvisorId(Guid advisorId, CancellationToken cancellationToken)
        {
            return _ctx.DoctorRepository.ExistsAsync(x => x.UserId == advisorId
                                                       && x.IsAcademicAdvisor == true, cancellationToken);
        }

        private async Task<bool> CheckIfOnOwnedSupportRequest(Guid requestId, CancellationToken cancellationToken)
        {
            var studentSupport = await _ctx.StudentSupportRepository
                .GetFirstIncludingAll(x => x.Id == requestId)
                .FirstOrDefaultAsync(cancellationToken);

            return studentSupport != null && studentSupport.Student.AcademicAdvisorId == _currentUserService.UserId;
        }
    }
}
