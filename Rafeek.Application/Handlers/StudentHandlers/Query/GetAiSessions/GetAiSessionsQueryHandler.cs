using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetAiSessions
{
    public class GetAiSessionsQueryHandler : IRequestHandler<GetAiSessionsQuery, List<AiSessionDto>>
    {
        private readonly IRafeekDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetAiSessionsQueryHandler(IRafeekDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<List<AiSessionDto>> Handle(GetAiSessionsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            // إذا كان المستخدم غير مسجل، ارجع قائمة فارغة (أو قم بإرجاع جميع الجلسات لأغراض الاختبار)
            if (userId == Guid.Empty)
            {
                return new List<AiSessionDto>();
            }

            var sessions = await _dbContext.ChatSessions
                .Where(s => s.UserId == userId)
                .Select(s => new AiSessionDto
                {
                    SessionId = s.Id,
                    SessionTitle = s.Title,
                    LastMessageAt = s.ChatbotQueries.Any() ? s.ChatbotQueries.Max(q => q.CreatedAt) : s.CreatedAt,
                    MessagesCount = s.ChatbotQueries.Count
                })
                .OrderByDescending(s => s.LastMessageAt)
                .ToListAsync(cancellationToken);

            return sessions;
        }
    }
}
