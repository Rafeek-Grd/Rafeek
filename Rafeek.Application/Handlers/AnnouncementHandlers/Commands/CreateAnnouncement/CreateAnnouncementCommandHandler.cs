using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, Guid>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public CreateAnnouncementCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                AudienceType = request.AudienceType,
                DepartmentId = request.DepartmentId,
                TargetLevel = request.TargetLevel,
                SendInApp = request.SendInApp,
                SendEmail = request.SendEmail,
                SendSMS = request.SendSMS,
                IsUrgent = request.IsUrgent,
                ScheduledAt = request.ScheduledAt,
                IsDeactivated = false,
                IsSent = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Admin",
                IsActive = true
            };

            _ctx.AnnouncementRepository.Add(announcement);
            await _ctx.SaveChangesAsync(cancellationToken);

            return announcement.Id;
        }
    }
}
