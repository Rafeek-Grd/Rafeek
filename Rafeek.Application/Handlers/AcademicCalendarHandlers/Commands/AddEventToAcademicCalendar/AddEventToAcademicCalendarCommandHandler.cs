using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.AddEventToAcademicCalendar
{
    public class AddEventToAcademicCalendarCommandHandler: IRequestHandler<AddEventToAcademicCalendarCommand, string>
    {
        private readonly IUnitOfWork _ctx;

        public AddEventToAcademicCalendarCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
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

            await _ctx.AcademicCalendarRepository.AddAsync(entity, cancellationToken);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? LocalizationKeys.GlobalValidationMessages.AddedSuccessfully : LocalizationKeys.GlobalValidationMessages.AddedFailed;
        }
    }
}
