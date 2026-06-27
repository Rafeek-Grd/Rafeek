using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AnnouncementHandlers.DTOs;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Queries.GetAnnouncementsPaginated
{
    public class GetAnnouncementsPaginatedQueryHandler 
        : IRequestHandler<GetAnnouncementsPaginatedQuery, PagginatedResult<AnnouncementDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAnnouncementsPaginatedQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<AnnouncementDto>> Handle(
            GetAnnouncementsPaginatedQuery request, 
            CancellationToken cancellationToken)
        {
            var query = _ctx.AnnouncementRepository.GetAll()
                .Include(x => x.Department)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(x => x.Title.Contains(request.SearchTerm) || x.Content.Contains(request.SearchTerm));
            }

            if (request.IsSent.HasValue)
            {
                query = query.Where(x => x.IsSent == request.IsSent.Value);
            }

            if (request.IsDeactivated.HasValue)
            {
                query = query.Where(x => x.IsDeactivated == request.IsDeactivated.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var orderedQuery = query.OrderByDescending(x => x.ScheduledAt).ThenByDescending(x => x.CreatedAt).AsQueryable();

            if (request.PageNumber != -1)
            {
                orderedQuery = orderedQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var entities = await orderedQuery.ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<AnnouncementDto>>(entities);

            return PagginatedResult<AnnouncementDto>.Create(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
