using MediatR;
using System;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.PostponeAnnouncement
{
    public class PostponeAnnouncementCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DateTime NewScheduledAt { get; set; }
    }
}
