using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Queries.GetCourseSectionsByCourse
{
    public class GetCourseSectionsByCourseQueryHandler : IRequestHandler<GetCourseSectionsByCourseQuery, List<CourseSectionDto>>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public GetCourseSectionsByCourseQueryHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseSectionDto>> Handle(GetCourseSectionsByCourseQuery request, CancellationToken cancellationToken)
        {
            var sections = await _context.CourseSections
                .AsNoTracking()
                .Where(x => x.CourseId == request.CourseId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CourseSectionDto>>(sections);
        }
    }
}
