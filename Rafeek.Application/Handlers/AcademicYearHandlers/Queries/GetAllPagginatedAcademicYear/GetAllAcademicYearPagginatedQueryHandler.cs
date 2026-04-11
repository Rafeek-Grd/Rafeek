using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAllAcademicYear
{
    public class GetAllAcademicYearsQueryHandler : IRequestHandler<GetAllAcademicYearsPagginatedQuery, PagginatedResult<AcademicYearDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAllAcademicYearsQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<AcademicYearDto>> Handle(GetAllAcademicYearsPagginatedQuery request, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicYearRepository
                .IncludeAll(null)
                .AsNoTracking()
                .ProjectTo<AcademicYearDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
