using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAllAcademicYear
{
    public class GetAllAcademicYearsQueryHandler : IRequestHandler<GetAllAcademicYearsQuery, List<AcademicYearDto>>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public GetAllAcademicYearsQueryHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AcademicYearDto>> Handle(GetAllAcademicYearsQuery request, CancellationToken cancellationToken)
        {
            return await _context.AcademicYears
                .AsNoTracking()
                .ProjectTo<AcademicYearDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
