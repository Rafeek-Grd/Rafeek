using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.CreateCourseSection
{
    public class CreateCourseSectionCommandHandler : IRequestHandler<CreateCourseSectionCommand, CourseSectionDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public CreateCourseSectionCommandHandler(IUnitOfWork ctx, IRafeekDbContext context, IMapper mapper)
        {
            _ctx = ctx;
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseSectionDto> Handle(CreateCourseSectionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<CourseSection>(request);

            _ctx.CourseSectionRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CourseSectionDto>(entity);
        }
    }
}
