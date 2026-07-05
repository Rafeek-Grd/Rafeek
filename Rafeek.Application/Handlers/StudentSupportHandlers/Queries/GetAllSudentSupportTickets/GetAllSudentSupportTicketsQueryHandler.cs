using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllSudentSupportTickets
{
    public class GetAllSudentSupportTicketsQueryHandler : IRequestHandler<GetAllSudentSupportTicketsQuery, List<NewStudentSupportDto>>
    {
        private readonly IUnitOfWork _ctx;

        public GetAllSudentSupportTicketsQueryHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<NewStudentSupportDto>> Handle(GetAllSudentSupportTicketsQuery request, CancellationToken cancellationToken)
        {
            return await _ctx.StudentSupportRepository
                .GetAll()
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NewStudentSupportDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Email = x.Email ?? string.Empty,
                    StudentSupportStatus = x.StudentSupportStatus,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync(cancellationToken);
        }
    }
}
