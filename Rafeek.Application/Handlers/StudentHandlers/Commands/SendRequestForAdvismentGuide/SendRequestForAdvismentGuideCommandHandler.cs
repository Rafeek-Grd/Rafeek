using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.SendRequestForAdvismentGuide
{
    public class SendRequestForAdvismentGuideCommandHandler : IRequestHandler<SendRequestForAdvismentGuideCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public SendRequestForAdvismentGuideCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(SendRequestForAdvismentGuideCommand request, CancellationToken cancellationToken)
        {
            var studentSupport = new StudentSupport()
            {
                StudentId = request.StudentId,
                Title = request.Title,
                Description = request.Description,
                StudentSupportStatus = StudentSupportStatus.Pending
            };

            _ctx.StudentSupportRepository.Add(studentSupport);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? 
                _localizer[LocalizationKeys.StudentSupport.RequestIsSentSuccessfully] : 
                _localizer[LocalizationKeys.StudentSupport.RequestIsFailedToSend];
        }
    }
}
