using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces;

namespace Rafeek.Application.Handlers.AuthHandlers.SendUserCredentials
{
    public class SendUserCredentialsCommandHandler : IRequestHandler<SendUserCredentialsCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<Messages> _localizer;

        public SendUserCredentialsCommandHandler(
            IUserRepository userRepository, 
            IStringLocalizer<Messages> localizer)
        {
            _userRepository = userRepository;
            _localizer = localizer;
        }

        public async Task<string> Handle(SendUserCredentialsCommand request, CancellationToken cancellationToken)
        {

            await _userRepository.SendUserCredientialsViaEmailAsync(request.Email, request.Password, cancellationToken);
            return _localizer[LocalizationKeys.EmailTemplates.SendUserCredentials.Message.Value];
        }
    }
}
