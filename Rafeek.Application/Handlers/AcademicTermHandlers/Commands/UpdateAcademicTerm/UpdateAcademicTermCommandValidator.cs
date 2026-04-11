using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.UpdateAcademicTerm
{
    public class UpdateAcademicTermCommandValidator: AbstractValidator<UpdateAcademicTermCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IRafeekDbContext _context;

        public UpdateAcademicTermCommandValidator(IStringLocalizer<Messages> localizer, IRafeekDbContext context)
        {
            _localizer = localizer;
            _context = context;

            RuleFor(x => x.Id)
                .CustomAsync(async (id, context, cancellationToken) =>
                {
                    if (!await AcademicTermExists(id, cancellationToken))
                    {
                        context.AddFailure(_localizer[LocalizationKeys.AcademicTerm.AcademicTermNotFound]);
                    }
                });
        }

        private async Task<bool> AcademicTermExists(Guid id, CancellationToken cancellationToken)
        {
            return await _context.AcademicTerms.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
