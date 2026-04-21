using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CareerHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CareerHandlers.Queries.GetCareerSuggestionsByStudent
{
    public class GetCareerSuggestionsByStudentQueryHandler : IRequestHandler<GetCareerSuggestionsByStudentQuery, PagginatedResult<CareerSuggestionDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetCareerSuggestionsByStudentQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<CareerSuggestionDto>> Handle(GetCareerSuggestionsByStudentQuery request, CancellationToken cancellationToken)
        {
            var suggestions = await _ctx.CareerSuggestionRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.StudentId == request.StudentId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<CareerSuggestionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return suggestions;
        }
    }
}
