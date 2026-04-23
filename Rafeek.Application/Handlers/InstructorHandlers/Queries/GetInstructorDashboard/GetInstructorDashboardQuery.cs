using MediatR;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQuery : IRequest<InstructorDashboardDto>
    {
    }
}
