using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ForegetPassword
{
    public class ForegetPasswordCommandHandler : IRequestHandler<ForegetPasswordCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<Messages> _localizer;

        public ForegetPasswordCommandHandler(IUserRepository userRepository, IStringLocalizer<Messages> localizer)
        {
            _userRepository = userRepository;
            _localizer = localizer;
        }

        public async Task<string> Handle(ForegetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.SendConfirmationCodeAsync(request.Email, cancellationToken);

            return _localizer[LocalizationKeys.EmailTemplates.ForgotPassword.Message.Value];
        }
    }
}
