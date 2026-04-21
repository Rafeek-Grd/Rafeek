using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.LearningResourceHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.LearningResourceHandlers.Queries.GetAllLearningResources
{
    public class GetAllLearningResourcesQueryHandlerPagginated : IRequestHandler<GetAllLearningResourcesQueryPagginated, PagginatedResult<LearningResourceDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAllLearningResourcesQueryHandlerPagginated(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<LearningResourceDto>> Handle(GetAllLearningResourcesQueryPagginated request, CancellationToken cancellationToken)
        {
            var query = _ctx.LearningResourceRepository.GetAll().AsNoTracking();

            if (request.ResourceType.HasValue)
            {
                query = query.Where(x => x.ResourceType == request.ResourceType.Value);
            }

            var resources = await query
                .OrderBy(x => x.Description)
                .ProjectTo<LearningResourceDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return resources;
        }
    }
}
