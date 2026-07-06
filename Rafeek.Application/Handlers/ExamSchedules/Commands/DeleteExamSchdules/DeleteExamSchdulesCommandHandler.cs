using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ExamSchedules.Commands.DeleteExamSchdules
{
    public class DeleteExamSchdulesCommandHandler : IRequestHandler<DeleteExamSchdulesCommand, string>
    {
        private readonly IUnitOfWork _ctx;

        public DeleteExamSchdulesCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<string> Handle(DeleteExamSchdulesCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                var entity = await _ctx.AcademicCalendarRepository.FindByKeyAsync(request.Id.Value, cancellationToken);
                entity!.IsDeleted = true;
                entity.IsActive = false;
                _ctx.AcademicCalendarRepository.Update(entity);
            }
            else
            {
                var all = await _ctx.AcademicCalendarRepository.GetAll().ToListAsync(cancellationToken);
                foreach (var item in all)
                {
                    item.IsDeleted = true;
                    item.IsActive = false;
                }
                _ctx.AcademicCalendarRepository.UpdateRange(all);
            }

            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? LocalizationKeys.GlobalValidationMessages.DeletedSuccessfully : LocalizationKeys.GlobalValidationMessages.DeletedFailed;
        }
    }
}