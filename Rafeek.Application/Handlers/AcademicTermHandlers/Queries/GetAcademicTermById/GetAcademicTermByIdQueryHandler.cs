using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries.GetAcademicTermById
{
    public class GetAcademicTermByIdQueryHandler : IRequestHandler<GetAcademicTermByIdQuery, AcademicTermDto?>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAcademicTermByIdQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<AcademicTermDto?> Handle(GetAcademicTermByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ctx.AcademicTermRepository
                .GetFirstIncludingAll(x => x.Id == request.Id)
                .ProjectTo<AcademicTermDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
