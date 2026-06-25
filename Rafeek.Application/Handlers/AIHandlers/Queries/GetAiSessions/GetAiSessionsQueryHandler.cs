using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAiSessions
{
    public class GetAiSessionsQueryHandler : IRequestHandler<GetAiSessionsQuery, PagginatedResult<AiSessionDto>>
    {
        private readonly IRafeekDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetAiSessionsQueryHandler(IRafeekDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<AiSessionDto>> Handle(GetAiSessionsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == Guid.Empty)
                return new PagginatedResult<AiSessionDto>(new List<AiSessionDto>(), 0, 1, 20);

            var query = _dbContext.ChatSessions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => new AiSessionDto
                {
                    SessionId = s.Id,
                    SessionTitle = s.Title,
                    LastMessageAt = s.ChatbotQueries.Any() ? s.ChatbotQueries.Max(q => q.CreatedAt) : s.CreatedAt,
                    MessagesCount = s.ChatbotQueries.Count
                })
                .OrderByDescending(s => s.LastMessageAt);

            List<AiSessionDto> items;
            int totalCount;

            if (request.PageNumber == -1)
            {
                items = await query.ToListAsync(cancellationToken);
                totalCount = items.Count;
            }
            else
            {
                totalCount = await query.CountAsync(cancellationToken);
                items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }

            return new PagginatedResult<AiSessionDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
