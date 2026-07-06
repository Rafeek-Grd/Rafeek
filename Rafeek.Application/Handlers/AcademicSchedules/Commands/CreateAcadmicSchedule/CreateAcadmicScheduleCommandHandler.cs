using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AcademicSchedules.DTOs;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.CreateAcadmicSchedule
{
    public class CreateAcadmicScheduleCommandHandler : IRequestHandler<CreateAcadmicScheduleCommand, AcademicScheduleDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IRafeekDbContext _context;

        public CreateAcadmicScheduleCommandHandler(IUnitOfWork ctx, IRafeekDbContext context)
        {
            _ctx = ctx;
            _context = context;
        }

        public async Task<AcademicScheduleDto> Handle(CreateAcadmicScheduleCommand request, CancellationToken cancellationToken)
        {
            var entity = new LectureGroup
            {
                CourseId = request.CourseId,
                DoctorId = request.DoctorId,
                Day = request.Day,
                Time = request.Time,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Capacity = request.Capacity,
                Location = request.Location
            };

            _ctx.LectureGroupRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            var section = await _context.LectureGroups
                .AsNoTracking()
                .Include(s => s.Course)
                .Include(s => s.Doctor)
                    .ThenInclude(d => d.User)
                .Include(s => s.CalendarEvents)
                .FirstAsync(s => s.Id == entity.Id, cancellationToken);

            var calendarEvent = section.CalendarEvents?.FirstOrDefault();
            var room = calendarEvent?.Location ?? "غير محدد";
            var status = calendarEvent?.Status.ToString() ?? "Published";

            return new AcademicScheduleDto
            {
                LectureGroupId = section.Id,
                CourseId = section.CourseId,
                CourseTitle = section.Course.Title,
                CourseCode = section.Course.Code,
                DoctorName = section.Doctor?.User?.FullName ?? "غير محدد",
                Room = room,
                Location = section.Location ?? room,
                Day = section.Day ?? "-",
                Time = section.Time ?? "-",
                Status = status
            };
        }
    }
}
