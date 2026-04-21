using MediatR;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetAiSessions
{
    public class GetAiSessionsQuery : IRequest<List<AiSessionDto>>
    {
    }
}
