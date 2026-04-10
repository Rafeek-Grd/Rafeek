using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommandHandler : IRequestHandler<UpdateAcademicYearCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public UpdateAcademicYearCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(UpdateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicYearRepository.GetFirstAsync(x => x.Id == request.Id, cancellationToken);


            if (request.Name is not null)
                entity.Name = request.Name;

            if (request.StartDate.HasValue)
                entity.StartDate = request.StartDate.Value;

            if (request.EndDate.HasValue)
                entity.EndDate = request.EndDate.Value;

            if (request.IsCurrentYear.HasValue)
                entity.IsCurrentYear = request.IsCurrentYear.Value;

            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
