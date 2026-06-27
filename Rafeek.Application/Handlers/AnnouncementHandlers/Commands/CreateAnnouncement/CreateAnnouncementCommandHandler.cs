using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, Guid>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;
        private readonly IEmailNotificationService _emailService;
        private readonly ISmsService _smsService;

        public CreateAnnouncementCommandHandler(
            IUnitOfWork ctx, 
            IMapper mapper,
            IEmailNotificationService emailService,
            ISmsService smsService)
        {
            _ctx = ctx;
            _mapper = mapper;
            _emailService = emailService;
            _smsService = smsService;
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

            // 1. Resolve target students based on audience filters
            var studentsQuery = _ctx.StudentRepository.GetAll()
                .Include(s => s.User)
                .AsNoTracking();

            if (announcement.AudienceType == 1) // Specific Department
            {
                studentsQuery = studentsQuery.Where(s => s.DepartmentId == announcement.DepartmentId);
            }
            else if (announcement.AudienceType == 2) // Academic Level
            {
                studentsQuery = studentsQuery.Where(s => s.Level == announcement.TargetLevel);
            }

            var targetStudents = await studentsQuery.ToListAsync(cancellationToken);

            // 2. Dispatch - In-App Notifications
            if (announcement.SendInApp)
            {
                foreach (var student in targetStudents)
                {
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        UserId = student.UserId,
                        Title = announcement.Title,
                        Message = announcement.Content,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System",
                        IsActive = true
                    };
                    _ctx.NotificationRepository.Add(notification);
                }
            }

            // 3. Dispatch - Email Broadcast
            if (announcement.SendEmail)
            {
                foreach (var student in targetStudents)
                {
                    if (!string.IsNullOrEmpty(student.User.Email))
                    {
                        try
                        {
                            await _emailService.SendRawEmailAsync(student.User.Email, announcement.Title, announcement.Content);
                        }
                        catch
                        {
                            // Silent fail for single recipient to allow loop to continue
                        }
                    }
                }
            }

            // 4. Dispatch - SMS Alerts
            if (announcement.SendSMS)
            {
                foreach (var student in targetStudents)
                {
                    if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                    {
                        try
                        {
                            await _smsService.SendSmsAsync(student.User.PhoneNumber, $"{announcement.Title}: {announcement.Content}");
                        }
                        catch
                        {
                            // Silent fail for single recipient to allow loop to continue
                        }
                    }
                }
            }

            // 5. Mark as sent immediately
            announcement.IsSent = true;

            _ctx.AnnouncementRepository.Add(announcement);
            await _ctx.SaveChangesAsync(cancellationToken);

            return announcement.Id;
        }
    }
}
