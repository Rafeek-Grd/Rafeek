using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.NewStudentSupportTicket
{
    public class NewStudentSupportTicketCommandValidator : AbstractValidator<NewStudentSupportTicketCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NewStudentSupportTicketCommandValidator(IStringLocalizer<Messages> localizer, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.StudentSupport.TitleRequired.Value]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.StudentSupport.DescriptionRequired.Value]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.Student.StudentIdRequired.Value])
                .EmailAddress().WithMessage(localizer[LocalizationKeys.ExceptionMessage.BadRequest.Value]);

            RuleFor(x => x.TicketType)
                .IsInEnum().WithMessage(localizer[LocalizationKeys.ExceptionMessage.BadRequest.Value]);
        }

        private async Task<bool> EmailExists(string email, CancellationToken cancellationToken)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}
