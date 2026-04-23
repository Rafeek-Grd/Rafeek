using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Commands.CreateAssignment
{
    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, Guid>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public CreateAssignmentCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = _mapper.Map<Assignment>(request);
            _ctx.AssignmentRepository.Add(assignment);
            await _ctx.SaveChangesAsync(cancellationToken);

            return assignment.Id;
        }
    }
}
