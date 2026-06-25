using FluentValidation;

namespace Rafeek.Application.Handlers.SettingsHandlers.Commands.UpdateAcademicSettings
{
    public class UpdateAcademicSettingsCommandValidator : AbstractValidator<UpdateAcademicSettingsCommand>
    {
        public UpdateAcademicSettingsCommandValidator()
        {
            RuleFor(x => x.MaxHoursPerSemester)
                .GreaterThan(0).WithMessage("الحد الأقصى للساعات للفصل الدراسي يجب أن يكون أكبر من الصفر.");

            RuleFor(x => x.CourseCreditHours)
                .GreaterThan(0).WithMessage("عدد ساعات المقرر يجب أن يكون أكبر من الصفر.");

            RuleFor(x => x.GradeScales)
                .NotEmpty().WithMessage("يجب إضافة تقدير مادة واحد على الأقل.");
        }
    }
}
