using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.UpdateEventOfAcademicCalendar
{
    public class UpdateEventOfAcademicCalendarCommandHandler : IRequestHandler<UpdateEventOfAcademicCalendarCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public UpdateEventOfAcademicCalendarCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<string> Handle(UpdateEventOfAcademicCalendarCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.AcademicCalendarRepository.FindByKeyAsync( request.Id , cancellationToken)
                ?? throw new NotFoundException(nameof(AcademicCalendar), request.Id);

            _mapper.Map(request, entity);

            _ctx.AcademicCalendarRepository.Update(entity);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ? LocalizationKeys.GlobalValidationMessages.UpdatedSuccessfully : LocalizationKeys.GlobalValidationMessages.UpdatedFailed;
        }
    }
}
