using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.PostponeAnnouncement
{
    public class PostponeAnnouncementCommandHandler : IRequestHandler<PostponeAnnouncementCommand, bool>
    {
        private readonly IUnitOfWork _ctx;

        public PostponeAnnouncementCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Handle(PostponeAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var announcement = await _ctx.AnnouncementRepository.GetAll()
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (announcement == null)
                throw new NotFoundException(nameof(Announcement), request.Id);

            announcement.ScheduledAt = request.NewScheduledAt;
            announcement.PostponedTo = request.NewScheduledAt;
            _ctx.AnnouncementRepository.Update(announcement);
            await _ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
