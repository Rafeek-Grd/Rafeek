using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAcademicYearById
{
    public class GetAcademicYearByIdQueryHandler : IRequestHandler<GetAcademicYearByIdQuery, AcademicYearDto?>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public GetAcademicYearByIdQueryHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AcademicYearDto?> Handle(GetAcademicYearByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.AcademicYears
                .Where(x => x.Id == request.Id)
                .ProjectTo<AcademicYearDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
