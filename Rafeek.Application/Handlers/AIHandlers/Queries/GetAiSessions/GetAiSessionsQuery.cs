using MediatR;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAiSessions
{
    public class GetAiSessionsQuery : IRequest<List<AiSessionDto>>
    {
    }
}
