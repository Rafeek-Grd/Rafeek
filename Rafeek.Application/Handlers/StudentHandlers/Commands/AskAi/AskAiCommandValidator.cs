using FluentValidation;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.AskAi
{
    public class AskAiCommandValidator : AbstractValidator<AskAiCommand>
    {
        public AskAiCommandValidator()
        {
            RuleFor(v => v.Question)
                .NotEmpty().WithMessage("السؤال مطلوب ولا يمكن أن يكون فارغاً.");
        }
    }
}
