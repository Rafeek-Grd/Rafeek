using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllSudentSupportTickets
{
    public class GetAllSudentSupportTicketsQuery : IRequest<List<NewStudentSupportDto>>
    {
    }
}
