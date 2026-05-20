using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetChatHistory
{
    public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, List<ChatHistoryDto>>
    {
        private readonly IRafeekDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetChatHistoryQueryHandler(IRafeekDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<List<ChatHistoryDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            // جلب الطالب عن طريق UserId
            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId || s.Id == userId, cancellationToken);

            if (student == null)
                return new List<ChatHistoryDto>();

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

            if (request.PageSize != -1)
            {
                projectedQuery = projectedQuery
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var history = await projectedQuery.ToListAsync(cancellationToken);

            return history;
        }
    }
}
