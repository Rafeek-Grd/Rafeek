using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Query.GetEventOfAcademicCalendarById
{
    public class GetEventOfAcademicCalendarByIdQueryHandler : IRequestHandler<GetEventOfAcademicCalendarByIdQuery, AcademicCalendarDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetEventOfAcademicCalendarByIdQueryHandler(
            IUnitOfWork ctx,
            IMapper mapper
        )
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<AcademicCalendarDto> Handle(GetEventOfAcademicCalendarByIdQuery request, CancellationToken cancellationToken)
        {
            var eventOfAcademicCalendar = await _ctx.AcademicCalendarRepository
            .GetFirstIncludingAll(x => x.Id == request.AcademicCalendarId)
            .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<AcademicCalendarDto>(eventOfAcademicCalendar);
        }
    }
}
