using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using AutoMapper.QueryableExtensions;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Query.GetAllPagginatedEvents
{
    public class GetAllEventsQueryPagginatedHandler : IRequestHandler<GetAllEventsPagginatedQuery, PagginatedResult<AcademicCalendarDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAllEventsQueryPagginatedHandler(
            IUnitOfWork ctx,
            IMapper mapper
        )
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<AcademicCalendarDto>> Handle(GetAllEventsPagginatedQuery request, CancellationToken cancellationToken)
        {
            return await _ctx
                .AcademicCalendarRepository
                .IncludeAll(null)
                .AsNoTracking()
                .ProjectTo<AcademicCalendarDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
