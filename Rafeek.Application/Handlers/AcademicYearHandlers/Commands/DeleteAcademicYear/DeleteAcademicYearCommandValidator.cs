using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.DeleteAcademicYear
{
    public class DeleteAcademicYearCommandValidator: AbstractValidator<DeleteAcademicYearCommand>
    {
        private readonly IRafeekDbContext _context;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteAcademicYearCommandValidator(IRafeekDbContext context, IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .CustomAsync(async (Id, context, cancellationToken) =>
                {
                    if (!await AcademicYearExistsBefore(Id, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.AcademicYear.AcademicYearNotFound].Value);
                    }
                });
        }

        private Task<bool> AcademicYearExistsBefore(Guid Id, CancellationToken cancellationToken)
        {
            return _context.AcademicYears.AnyAsync(x => x.Id == Id, cancellationToken);
        }
    } 
}
