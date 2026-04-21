using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.DeleteCourse
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly IUnitOfWork _ctx;

        public DeleteCourseCommandHandler(IUnitOfWork _ctx)
        {
            this._ctx = _ctx;
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.CourseRepository.FindByKeyAsync(request.Id, cancellationToken);

            _ctx.CourseRepository.Delete(entity);
            
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
