using AutoMapper;
using MediatR;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommandHandler : IRequestHandler<UpdateAcademicYearCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public UpdateAcademicYearCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicYearRepository.GetFirstAsync(x => x.Id == request.Id, cancellationToken);

            _mapper.Map(request, entity);

            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
