using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommandValidator: AbstractValidator<UpdateAcademicYearCommand>
    {
        private readonly IRafeekDbContext _context;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateAcademicYearCommandValidator(IRafeekDbContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .CustomAsync(async (id, context, cancellationToken) =>
                {
                    if(!await AcademicYearExistsBefore(id, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.AcademicYear.AcademicYearNotFound.Value]);
                    }
                });
        }

        private Task<bool> AcademicYearExistsBefore(Guid id, CancellationToken cancellationToken)
        {
            return _context.AcademicYears.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
