using MediatR;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.DeleteAcademicTerm
{
    public class DeleteAcademicTermCommandHandler : IRequestHandler<DeleteAcademicTermCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public DeleteAcademicTermCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(DeleteAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicTermRepository.GetFirstAsync(x => x.Id == request.Id, cancellationToken);

            _ctx.AcademicTermRepository.Delete(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
