using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Net.Http.Headers;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.DeleteAcademicTerm
{
    public class DeleteAcademicTermCommandHandler : IRequestHandler<DeleteAcademicTermCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteAcademicTermCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicTermRepository.GetFirstAsync(x => x.Id == request.Id, cancellationToken);

            entity.IsActive = false;
            entity.IsDeleted = true;

            _ctx.AcademicTermRepository.Update(entity);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? Unit.Value 
                : throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.DeletedFailed.Value]);
        }
    }
}
