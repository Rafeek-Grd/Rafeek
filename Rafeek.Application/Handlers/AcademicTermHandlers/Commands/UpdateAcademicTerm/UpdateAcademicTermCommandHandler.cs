using AutoMapper;
using MediatR;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.UpdateAcademicTerm
{
    public class UpdateAcademicTermCommandHandler : IRequestHandler<UpdateAcademicTermCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public UpdateAcademicTermCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicTermRepository.GetFirstAsync(x => x.Id == request.Id, cancellationToken);

            _mapper.Map(request, entity);

            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
