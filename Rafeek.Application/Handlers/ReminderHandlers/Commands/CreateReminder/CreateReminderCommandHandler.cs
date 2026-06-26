using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
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
        private readonly IIdentityUnitOfWork _identityUnitOfWork;

        public CreateReminderCommandHandler(IUnitOfWork ctx, IMapper mapper, ICurrentUserService currentUserService, IIdentityUnitOfWork identityUnitOfWork)
        {
            _ctx = ctx;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityUnitOfWork = identityUnitOfWork;
        }

        public async Task<Unit> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == System.Guid.Empty)
                throw new UnauthorizedException("User is not authenticated.");

            var userExists = await _identityUnitOfWork.ApplicationUserRepository
                .GetAll()
                .AsNoTracking()
                .AnyAsync(u => u.Id == _currentUserService.UserId, cancellationToken);

            if (!userExists)
                throw new NotFoundException($"User with ID {_currentUserService.UserId} was not found.");

            var entity = _mapper.Map<Reminder>(request);
            entity.UserId = _currentUserService.UserId;

            _ctx.ReminderRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
