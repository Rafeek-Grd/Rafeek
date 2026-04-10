using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.DeleteAcademicYear
{
    public class DeleteAcademicYearCommandHandler : IRequestHandler<DeleteAcademicYearCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public DeleteAcademicYearCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(DeleteAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicYearRepository.GetFirstAsync(x => x.Id == request.Id);

            _ctx.AcademicYearRepository.Delete(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
