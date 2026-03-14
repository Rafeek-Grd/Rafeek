using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Extensions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.DeleteEventOfAcademicCalendar
{
    public class DeleteEventOfAcademicCalendarCommandHandler : IRequestHandler<DeleteEventOfAcademicCalendarCommand, string>
    {
        private readonly IUnitOfWork _ctx;

        public DeleteEventOfAcademicCalendarCommandHandler(
            IUnitOfWork ctx
        )
        {
            _ctx = ctx;
        }

        public async Task<string> Handle(DeleteEventOfAcademicCalendarCommand request, CancellationToken cancellationToken)
        {
            var academicEvent = await _ctx
                .AcademicCalendarRepository
                .GetAll()
                .FirstOrDefaultAsync(ac => ac.Id == request.AcademicEventId.ToGuid(), cancellationToken)
                ?? throw new NotFoundException(nameof(AcademicCalendar), request.AcademicEventId);

            academicEvent.IsActive = false;
            academicEvent.IsDeleted = true;

            _ctx.AcademicCalendarRepository.Update(academicEvent);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? 
                LocalizationKeys.GlobalValidationMessages.DeletedSuccessfully :
                LocalizationKeys.GlobalValidationMessages.DeletedFailed;
        }
    }
}
