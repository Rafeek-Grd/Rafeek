using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries.GetAllPagginatedAcademicTerm
{
    public class GetAllAcademicTermPagginatedQueryHandler : IRequestHandler<GetAllAcademicTermPagginatedQuery, PagginatedResult<AcademicTermDto>>
    {
        private readonly IUnitOfWork _ctx;
        private IMapper _mapper;

        public GetAllAcademicTermPagginatedQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<AcademicTermDto>> Handle(GetAllAcademicTermPagginatedQuery request, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicTermRepository
               .IncludeAll(null)
               .AsNoTracking()
               .ProjectTo<AcademicTermDto>(_mapper.ConfigurationProvider)
               .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
