using AutoMapper;
using MediatR;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.AddAcademicYearCommand
{
    public class AddAcademicYearCommandHandler : IRequestHandler<AddAcademicYearCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public AddAcademicYearCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<AcademicYear>(request);

            _ctx.AcademicYearRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
