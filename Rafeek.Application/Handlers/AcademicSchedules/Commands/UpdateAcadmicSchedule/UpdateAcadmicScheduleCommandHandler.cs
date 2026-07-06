using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.UpdateAcadmicSchedule
{
    public class UpdateAcadmicScheduleCommandHandler : IRequestHandler<UpdateAcadmicScheduleCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateAcadmicScheduleCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(UpdateAcadmicScheduleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.LectureGroupRepository.FindByKeyAsync(request.LectureId, cancellationToken);

            if (request.CourseId.HasValue)
                entity!.CourseId = request.CourseId.Value;
            if (request.DoctorId.HasValue)
                entity!.DoctorId = request.DoctorId;
            if (request.Day is not null)
                entity!.Day = request.Day;
            if (request.Time is not null)
                entity!.Time = request.Time;
            if (request.StartTime.HasValue)
                entity!.StartTime = request.StartTime;
            if (request.EndTime.HasValue)
                entity!.EndTime = request.EndTime;
            if (request.Capacity.HasValue)
                entity!.Capacity = request.Capacity.Value;
            if (request.Location is not null)
                entity!.Location = request.Location;

            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0
                ? _localizer[LocalizationKeys.GlobalValidationMessages.UpdatedSuccessfully.Value]
                : _localizer[LocalizationKeys.GlobalValidationMessages.UpdatedFailed.Value];
        }
    }
}
