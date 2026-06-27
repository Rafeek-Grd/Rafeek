using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateAnnouncementCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("العنوان مطلوب")
                .MaximumLength(200).WithMessage("العنوان يجب ألا يتجاوز 200 حرف");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("المحتوى مطلوب");

            RuleFor(x => x.AudienceType)
                .InclusiveBetween(0, 2).WithMessage("فئة الجمهور المستهدف غير صالحة");

            RuleFor(x => x.ScheduledAt)
                .NotEmpty().WithMessage("تاريخ الجدولة مطلوب");
        }
    }
}
