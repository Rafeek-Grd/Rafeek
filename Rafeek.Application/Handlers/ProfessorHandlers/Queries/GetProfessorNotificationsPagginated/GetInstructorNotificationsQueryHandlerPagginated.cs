using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorNotifications
{
    public class GetInstructorNotificationsQueryHandlerPagginated : IRequestHandler<GetInstructorNotificationsQueryPagginated, PagginatedResult<InstructorNotificationDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;

        public GetInstructorNotificationsQueryHandlerPagginated(IUnitOfWork ctx, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<InstructorNotificationDto>> Handle(GetInstructorNotificationsQueryPagginated request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            return await _ctx.NotificationRepository
                .GetAll(n => n.UserId == userId)
                .AsNoTracking()
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new InstructorNotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Body = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
