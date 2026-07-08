using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
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
        private readonly IMapper _mapper;

        public CreateAcadmicScheduleCommandHandler(IUnitOfWork ctx, IRafeekDbContext context, IMapper mapper)
        {
            _ctx = ctx;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AcademicScheduleDto> Handle(CreateAcadmicScheduleCommand request, CancellationToken cancellationToken)
        {
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == request.CourseId, cancellationToken);
            if (!courseExists)
                throw new NotFoundException(nameof(Course), request.CourseId);

            if (request.DoctorId.HasValue)
            {
                var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == request.DoctorId.Value, cancellationToken);
                if (!doctorExists)
                    throw new NotFoundException(nameof(Doctor), request.DoctorId.Value);
            }

            var entity = _mapper.Map<LectureGroup>(request);

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
