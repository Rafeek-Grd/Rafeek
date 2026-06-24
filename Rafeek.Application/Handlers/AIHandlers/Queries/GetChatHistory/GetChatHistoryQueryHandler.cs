using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetChatHistory
{
    public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, PagginatedResult<ChatHistoryDto>>
    {
        private readonly IRafeekDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetChatHistoryQueryHandler(IRafeekDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<ChatHistoryDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId || s.Id == userId, cancellationToken);

            if (student == null)
                return new PagginatedResult<ChatHistoryDto>(new List<ChatHistoryDto>(), 0, 1, 20);

            var query = _dbContext.ChatbotQueries
                .AsNoTracking()
                .Where(q => q.StudentId == student.Id);

            if (request.SessionId.HasValue && request.SessionId != Guid.Empty)
            {
                query = query.Where(q => q.SessionId == request.SessionId.Value);
            }

            var orderedQuery = query.OrderByDescending(q => q.CreatedAt);

            var projectedQuery = orderedQuery.Select(q => new ChatHistoryDto
            {
                Id = q.Id,
                SessionId = q.SessionId,
                Question = q.Query,
                Answer = q.Response,
                AskedAt = q.CreatedAt
            });

            List<ChatHistoryDto> items;
            int totalCount;

            if (request.PageNumber == -1)
            {
                items = await projectedQuery.ToListAsync(cancellationToken);
                totalCount = items.Count;
            }
            else
            {
                totalCount = await projectedQuery.CountAsync(cancellationToken);
                items = await projectedQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }

            return new PagginatedResult<ChatHistoryDto>(items.AsReadOnly(), totalCount, request.PageNumber, request.PageSize);
        }
    }
}
