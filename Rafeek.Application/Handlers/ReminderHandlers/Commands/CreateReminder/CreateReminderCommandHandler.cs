using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.CreateReminder
{
    public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateReminderCommandHandler(IUnitOfWork ctx, IMapper mapper, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Reminder>(request);
            
            if (_currentUserService.UserId != System.Guid.Empty)
            {
                entity.UserId = _currentUserService.UserId;
            }

            _ctx.ReminderRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
