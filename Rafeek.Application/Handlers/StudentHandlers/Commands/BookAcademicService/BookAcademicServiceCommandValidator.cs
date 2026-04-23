using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.BookAcademicService
{
    public class BookAcademicServiceCommandValidator : AbstractValidator<BookAcademicServiceCommand>
    {
        public BookAcademicServiceCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.ServiceType)
                .IsInEnum().WithMessage(localizer["RequiredField_Msg"]);
                
            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage(localizer["RequiredField_Msg"]);
                
            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage(localizer["RequiredField_Msg"]);
        }
    }
}
