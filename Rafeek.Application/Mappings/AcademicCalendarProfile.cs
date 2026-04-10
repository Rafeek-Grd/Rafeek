using AutoMapper;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.UpdateEventOfAcademicCalendar;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class AcademicCalendarProfile: Profile
    {
        public AcademicCalendarProfile()
        {
            CreateMap<UpdateEventOfAcademicCalendarCommand, AcademicCalendar>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AcademicCalendar, AcademicCalendarDto>();
        }
    }
}
