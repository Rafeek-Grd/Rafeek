using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.NewStudentSupportTicket
{
    public class NewStudentSupportTicketCommandHandler : IRequestHandler<NewStudentSupportTicketCommand, NewStudentSupportDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;

        public NewStudentSupportTicketCommandHandler(IUnitOfWork ctx, ICurrentUserService currentUserService, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<NewStudentSupportDto> Handle(NewStudentSupportTicketCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated && request.TicketType != StudentSupportType.Account)
                throw new BadRequestException(_localizer[LocalizationKeys.ExceptionMessage.Unauthorized.Value]);

            var entity = new StudentSupport
            {
                Title = request.Title,
                Description = request.Description,
                Email = request.Email,
                TicketType = request.TicketType,
                StudentSupportStatus = StudentSupportStatus.Pending
            };

            await _ctx.StudentSupportRepository.AddAsync(entity, cancellationToken);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                throw new BadRequestException(_localizer[LocalizationKeys.StudentSupport.RequestIsFailedToSend.Value]);

            return new NewStudentSupportDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Email = entity.Email ?? string.Empty,
                TicketType = entity.TicketType,
                StudentSupportStatus = entity.StudentSupportStatus,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
