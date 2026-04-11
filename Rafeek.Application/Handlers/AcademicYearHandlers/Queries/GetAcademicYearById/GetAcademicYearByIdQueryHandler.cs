using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAcademicYearById
{
    public class GetAcademicYearByIdQueryHandler : IRequestHandler<GetAcademicYearByIdQuery, AcademicYearDto?>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAcademicYearByIdQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<AcademicYearDto?> Handle(GetAcademicYearByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicYearRepository
                .GetFirstIncludingAll(x => x.Id == request.Id)
                .ProjectTo<AcademicYearDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
