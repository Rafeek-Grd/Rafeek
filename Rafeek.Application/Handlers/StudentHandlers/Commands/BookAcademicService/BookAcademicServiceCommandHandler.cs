using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.BookAcademicService
{
    public class BookAcademicServiceCommandHandler : IRequestHandler<BookAcademicServiceCommand, Guid>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;

        public BookAcademicServiceCommandHandler(
            IRafeekDbContext context,
            ICurrentUserService currentUserService,
            IStringLocalizer<Messages> localizer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Guid> Handle(BookAcademicServiceCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            
            var student = await _context.Students
                .Include(s => s.AcademicAdvisor)
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);
                
            if (student == null)
            {
                throw new NotFoundException(nameof(Student), userId);
            }
                
            if (student.AcademicAdvisorId == null)
            {
                throw new BadRequestException("ليس لديك مرشد أكاديمي معين لإتمام إجراءات الحجز. يرجى التواصل مع شئون الطلاب أو إدارة القسم.");
            }
            
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                StudentId = student.Id,
                DoctorId = student.AcademicAdvisorId.Value,
                Status = AppointmentStatus.Scheduled,
                AppointmentDate = request.AppointmentDate.Date,
                StartTime = request.Time,
                EndTime = request.Time.Add(TimeSpan.FromMinutes(30)),
                Location = "المكتب الأكاديمي",
                Notes = $"نوع الخدمة: {request.ServiceType}\nالسبب: {request.Notes}"
            };
            
            await _context.Appointments.AddAsync(appointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return appointment.Id;
        }
    }
}
