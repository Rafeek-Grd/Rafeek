using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AssignmentHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Queries.GetAssignmentsBySection
{
    public class GetAssignmentsBySectionQueryHandlerPaginated : IRequestHandler<GetAssignmentsBySectionQueryPaginated, PagginatedResult<AssignmentDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAssignmentsBySectionQueryHandlerPaginated(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<AssignmentDto>> Handle(GetAssignmentsBySectionQueryPaginated request, CancellationToken cancellationToken)
        {
            return await _ctx.AssignmentRepository
                .GetAll(a => a.SectionId == request.SectionId && a.IsActive)
                .AsNoTracking()
                .OrderByDescending(a => a.DueDate)
                .ProjectTo<AssignmentDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
