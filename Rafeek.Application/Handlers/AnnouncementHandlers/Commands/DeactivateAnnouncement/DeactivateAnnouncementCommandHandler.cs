using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.DeactivateAnnouncement
{
    public class DeactivateAnnouncementCommandHandler : IRequestHandler<DeactivateAnnouncementCommand, bool>
    {
        private readonly IUnitOfWork _ctx;

        public DeactivateAnnouncementCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Handle(DeactivateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var announcement = await _ctx.AnnouncementRepository.GetAll()
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (announcement == null)
                throw new NotFoundException(nameof(Announcement), request.Id);

            announcement.IsDeactivated = true;
            _ctx.AnnouncementRepository.Update(announcement);
            await _ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
