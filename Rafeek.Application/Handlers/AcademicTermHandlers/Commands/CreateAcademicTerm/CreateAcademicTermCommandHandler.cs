using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.CreateAcademicTerm
{
    public class CreateAcademicTermCommandHandler : IRequestHandler<CreateAcademicTermCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public CreateAcademicTermCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<AcademicTerm>(request);

            _ctx.AcademicTermRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
