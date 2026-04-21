using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail
{
    public class GetCourseDetailQueryValidator: AbstractValidator<GetCourseDetailQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetCourseDetailQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Course.IdRequired.Value])
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.Course.NotFound.Value]);
        }

        private async Task<bool> CourseExists(Guid courseId, CancellationToken cancellationToken)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
        }
    }
}
