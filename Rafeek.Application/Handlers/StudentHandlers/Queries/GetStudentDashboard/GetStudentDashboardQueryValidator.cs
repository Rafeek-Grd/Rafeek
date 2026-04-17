using FluentValidation;

namespace Rafeek.Application.Handlers.StudentHandlers.Queries.GetStudentDashboard
{
    public class GetStudentDashboardQueryValidator : AbstractValidator<GetStudentDashboardQuery>
    {
        public GetStudentDashboardQueryValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("رقم المستخدم (User ID) مطلوب ولا يمكن أن يكون فارغاً.");
        }
    }
}
