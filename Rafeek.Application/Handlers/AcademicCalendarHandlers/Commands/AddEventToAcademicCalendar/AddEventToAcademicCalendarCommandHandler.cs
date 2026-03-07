using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.AddEventToAcademicCalendar
{
    public class AddEventToAcademicCalendarCommandHandler: IRequestHandler<AddEventToAcademicCalendarCommand, string>
    {
        private readonly IRafeekDbContext _dbContext;

        public AddEventToAcademicCalendarCommandHandler(IRafeekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Handle(AddEventToAcademicCalendarCommand request, CancellationToken cancellationToken)
        {
            var entity = new AcademicCalendar
            {
                EventName = request.EventName,
                Description = request.Description,
                EventDate = request.EventDate,
                EndDate = request.EndDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsAllDay = request.IsAllDay,
                Location = request.Location,
                EventType = request.EventType,
                Status = request.Status,
                Visibility = request.Visibility,
                RecurrenceType = request.RecurrenceType,
                RecurrenceEndDate = request.RecurrenceEndDate,
                TargetUserId = request.TargetUserId,
                AcademicTermId = request.AcademicTermId,
                DepartmentId = request.DepartmentId,
                CourseId = request.CourseId,
                SectionId = request.SectionId
            };

            await _dbContext.AcademicCalendars.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id.ToString();
        }
    }
}
