using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.DeleteAcademicSchedule
{
    public class DeleteAcademicScheduleCommandHandler : IRequestHandler<DeleteAcademicScheduleCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteAcademicScheduleCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(DeleteAcademicScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request.LectureId.HasValue)
            {
                var entity = await _ctx.LectureGroupRepository.FindByKeyAsync(request.LectureId.Value, cancellationToken);
                entity!.IsDeleted = true;
                entity.IsActive = false;
                _ctx.LectureGroupRepository.Update(entity);
            }
            else
            {
                var all = await _ctx.LectureGroupRepository.GetAll().ToListAsync(cancellationToken);
                foreach (var item in all)
                {
                    item.IsDeleted = true;
                    item.IsActive = false;
                }
                _ctx.LectureGroupRepository.UpdateRange(all);
            }

            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0
                ? _localizer[LocalizationKeys.GlobalValidationMessages.DeletedSuccessfully.Value]
                : _localizer[LocalizationKeys.GlobalValidationMessages.DeletedFailed.Value];
        }
    }
}
