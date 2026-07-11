using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Queries.GetCourseSectionsByCourse
{
    public class GetCourseSectionsByCourseQueryValidator : AbstractValidator<GetCourseSectionsByCourseQuery>
    {
        public GetCourseSectionsByCourseQueryValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(localizer["CourseSection_CourseIdRequired"]);
        }
    }
}
