using MediatR;
using System;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.DeactivateAnnouncement
{
    public class DeactivateAnnouncementCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
