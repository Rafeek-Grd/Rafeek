using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries
{
    public class GetAcademicTermByIdQuery : IRequest<AcademicTermDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetAcademicTermByIdQueryHandler : IRequestHandler<GetAcademicTermByIdQuery, AcademicTermDto?>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public GetAcademicTermByIdQueryHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AcademicTermDto?> Handle(GetAcademicTermByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.AcademicTerms
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ProjectTo<AcademicTermDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
