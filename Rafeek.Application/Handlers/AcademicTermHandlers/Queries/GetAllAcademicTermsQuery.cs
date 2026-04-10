using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries
{
    public class GetAllAcademicTermsQuery : IRequest<List<AcademicTermDto>>
    {
    }

    public class GetAllAcademicTermsQueryHandler : IRequestHandler<GetAllAcademicTermsQuery, List<AcademicTermDto>>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public GetAllAcademicTermsQueryHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AcademicTermDto>> Handle(GetAllAcademicTermsQuery request, CancellationToken cancellationToken)
        {
            return await _context.AcademicTerms
                .AsNoTracking()
                .ProjectTo<AcademicTermDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
