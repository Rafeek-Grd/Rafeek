using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.DeleteCourseSection
{
    public class DeleteCourseSectionCommandHandler : IRequestHandler<DeleteCourseSectionCommand, bool>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IRafeekDbContext _context;

        public DeleteCourseSectionCommandHandler(IUnitOfWork ctx, IRafeekDbContext context)
        {
            _ctx = ctx;
            _context = context;
        }

        public async Task<bool> Handle(DeleteCourseSectionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.CourseSections
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                return false;

            _ctx.CourseSectionRepository.Delete(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
