using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseById
{
    public class GetCourseByIdQueryValidator: AbstractValidator<GetCourseByIdQuery>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetCourseByIdQueryValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Course.IdRequired.Value])
                .MustAsync(CourseExists).WithMessage(_localizer[LocalizationKeys.Course.NotFound.Value]);
        }

        private async Task<bool> CourseExists(Guid courseId, CancellationToken cancellationToken)
        {
            return await _ctx.CourseRepository.ExistsAsync(c => c.Id == courseId, cancellationToken);
        }
    }
}
