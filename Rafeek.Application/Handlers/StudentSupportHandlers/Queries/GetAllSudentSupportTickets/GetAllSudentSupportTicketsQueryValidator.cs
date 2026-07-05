using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllSudentSupportTickets
{
    public class GetAllSudentSupportTicketsQueryValidator : AbstractValidator<GetAllSudentSupportTicketsQuery>
    {
        public GetAllSudentSupportTicketsQueryValidator(IStringLocalizer<Messages> localizer)
        {
        }
    }
}
