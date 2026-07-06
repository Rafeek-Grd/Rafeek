using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.UpdateExamSchdules
{
    public class UpdateExamSchdulesCommandHandler : IRequestHandler<UpdateExamSchdulesCommand, string>
    {
        private readonly IUnitOfWork _ctx;

        public UpdateExamSchdulesCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<string> Handle(UpdateExamSchdulesCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicCalendarRepository.FindByKeyAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(AcademicCalendar), request.Id);

            if (request.CourseId.HasValue)
                entity.CourseId = request.CourseId.Value;
            if (request.EventDate.HasValue)
                entity.EventDate = request.EventDate.Value;
            if (request.StartTime.HasValue)
                entity.StartTime = request.StartTime.Value;
            if (request.EndTime.HasValue)
                entity.EndTime = request.EndTime.Value;
            if (request.Location is not null)
                entity.Location = request.Location;
            if (request.Description is not null)
                entity.Description = request.Description;
            if (request.AcademicTermId.HasValue)
                entity.AcademicTermId = request.AcademicTermId;
            if (request.LectureGroupId.HasValue)
                entity.LectureGroupId = request.LectureGroupId;
            if (request.Status.HasValue)
                entity.Status = request.Status.Value;

            _ctx.AcademicCalendarRepository.Update(entity);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? LocalizationKeys.GlobalValidationMessages.UpdatedSuccessfully : LocalizationKeys.GlobalValidationMessages.UpdatedFailed;
        }
    }
}