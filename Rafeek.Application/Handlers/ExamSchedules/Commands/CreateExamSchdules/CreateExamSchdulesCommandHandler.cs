using MediatR;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.CreateExamSchdules
{
    public class CreateExamSchdulesCommandHandler : IRequestHandler<CreateExamSchdulesCommand, string>
    {
        private readonly IUnitOfWork _ctx;

        public CreateExamSchdulesCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<string> Handle(CreateExamSchdulesCommand request, CancellationToken cancellationToken)
        {
            var entity = new AcademicCalendar
            {
                EventName = $"امتحان {request.EventDate:yyyy-MM-dd}",
                Description = request.Description,
                EventDate = request.EventDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Location = request.Location,
                EventType = AcademicCalendarEventType.Exam,
                Status = request.Status,
                Visibility = EventVisibility.All,
                CourseId = request.CourseId,
                AcademicTermId = request.AcademicTermId,
                LectureGroupId = request.LectureGroupId
            };

            await _ctx.AcademicCalendarRepository.AddAsync(entity, cancellationToken);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? LocalizationKeys.GlobalValidationMessages.AddedSuccessfully : LocalizationKeys.GlobalValidationMessages.AddedFailed;
        }
    }
}