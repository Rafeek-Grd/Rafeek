using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AdvisorHandlers.DTOs;
using Rafeek.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Queries
{
    public class GetPendingGuidanceRequestsQuery : IRequest<List<GuidanceRequestDto>>
    {
        public Guid AdvisorId { get; set; }
    }

    public class GetPendingGuidanceRequestsQueryHandler : IRequestHandler<GetPendingGuidanceRequestsQuery, List<GuidanceRequestDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetPendingGuidanceRequestsQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<List<GuidanceRequestDto>> Handle(GetPendingGuidanceRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _context.StudentSupports
                .AsNoTracking()
                .Include(x => x.Student)
                .Where(x => x.Student.AcademicAdvisorId == request.AdvisorId && x.StudentSupportStatus == StudentSupportStatus.Pending)
                .Select(x => new GuidanceRequestDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StudentSupportStatus = x.StudentSupportStatus,
                    StudentId = x.StudentId,
                    StudentUniversityCode = x.Student.UniversityCode,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
