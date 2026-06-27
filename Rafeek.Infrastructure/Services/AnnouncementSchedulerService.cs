using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Infrastructure.Services
{
    public class AnnouncementSchedulerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AnnouncementSchedulerService> _logger;

        public AnnouncementSchedulerService(
            IServiceProvider serviceProvider,
            ILogger<AnnouncementSchedulerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Announcement Scheduler Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingAnnouncementsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing pending announcements.");
                }

                // Poll every 60 seconds
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }

            _logger.LogInformation("Announcement Scheduler Service is stopping.");
        }

        private async Task ProcessPendingAnnouncementsAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();
                var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

                // Get pending unsent, active announcements whose scheduled time has passed
                var pendingAnnouncements = await unitOfWork.AnnouncementRepository.GetAll()
                    .Where(a => !a.IsSent && !a.IsDeactivated && a.ScheduledAt <= DateTime.UtcNow)
                    .ToListAsync(cancellationToken);

                if (!pendingAnnouncements.Any())
                    return;

                _logger.LogInformation($"Found {pendingAnnouncements.Count} pending announcements to process.");

                foreach (var announcement in pendingAnnouncements)
                {
                    try
                    {
                        // 1. Resolve target students based on audience filters
                        var studentsQuery = unitOfWork.StudentRepository.GetAll()
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

                        _logger.LogInformation($"Broadcasting announcement '{announcement.Title}' to {targetStudents.Count} students.");

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
                                unitOfWork.NotificationRepository.Add(notification);
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
                                        await emailService.SendRawEmailAsync(student.User.Email, announcement.Title, announcement.Content);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, $"Failed to send announcement email to {student.User.Email}");
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
                                        await smsService.SendSmsAsync(student.User.PhoneNumber, $"{announcement.Title}: {announcement.Content}");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, $"Failed to send announcement SMS to {student.User.PhoneNumber}");
                                    }
                                }
                            }
                        }

                        // 5. Mark as sent
                        announcement.IsSent = true;
                        unitOfWork.AnnouncementRepository.Update(announcement);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error occurred while processing announcement {announcement.Id}");
                    }
                }
        }
    }
}
