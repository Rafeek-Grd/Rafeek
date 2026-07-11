using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.UpdateCourseSection
{
    public class UpdateCourseSectionCommandHandler : IRequestHandler<UpdateCourseSectionCommand, CourseSectionDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCourseSectionCommandHandler(IUnitOfWork ctx, IRafeekDbContext context, IMapper mapper)
        {
            _ctx = ctx;
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseSectionDto> Handle(UpdateCourseSectionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.CourseSections
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                throw new NotFoundException(nameof(Domain.Entities.CourseSection), request.Id);

            _mapper.Map(request, entity);

            _ctx.CourseSectionRepository.Update(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CourseSectionDto>(entity);
        }
    }
}
