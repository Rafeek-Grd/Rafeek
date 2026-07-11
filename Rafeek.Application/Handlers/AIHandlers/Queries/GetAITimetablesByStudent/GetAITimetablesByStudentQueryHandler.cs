using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.AIHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAITimetablesByStudent
{
    public class GetAITimetablesByStudentQueryHandler : IRequestHandler<GetAITimetablesByStudentQuery, List<AITimetableDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAITimetablesByStudentQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<AITimetableDto>> Handle(GetAITimetablesByStudentQuery request, CancellationToken cancellationToken)
        {
            var timetables = await _ctx.AITimetableRepository.GetAll()
                .Include(x => x.Items)
                .Where(x => x.StudentId == request.StudentId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<AITimetableDto>>(timetables);
        }
    }
}
