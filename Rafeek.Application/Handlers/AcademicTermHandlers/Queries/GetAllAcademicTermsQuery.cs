using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries
{
    public class GetAllAcademicTermsQuery : IRequest<PagginatedResult<AcademicTermDto>>
    {
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllAcademicTermsQueryHandler : IRequestHandler<GetAllAcademicTermsQuery, PagginatedResult<AcademicTermDto>>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public GetAllAcademicTermsQueryHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<AcademicTermDto>> Handle(GetAllAcademicTermsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.AcademicTerms
                .AsNoTracking()
                .ProjectTo<AcademicTermDto>(_mapper.ConfigurationProvider);

            return await query.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
