using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudyPlanHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudyPlanHandlers.Queries.GetStudyPlanByStudent
{
    public class GetStudyPlanByStudentQueryHandlerPagginated : IRequestHandler<GetStudyPlanByStudentQueryPagginated, PagginatedResult<StudyPlanDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetStudyPlanByStudentQueryHandlerPagginated(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<StudyPlanDto>> Handle(GetStudyPlanByStudentQueryPagginated request, CancellationToken cancellationToken)
        {
            var plans = await _ctx.StudyPlanRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.StudentId == request.StudentId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<StudyPlanDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return plans;
        }
    }
}
