using AutoMapper;
using Rafeek.Domain.Entities;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands.AddAcademicYearCommand;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands.UpdateAcademicYear;

namespace Rafeek.Application.Handlers.AcademicYearHandlers
{
    public class AcademicYearMapper : Profile
    {
        public AcademicYearMapper()
        {
            CreateMap<AcademicYear, AcademicYearDto>()
                .ForMember(dest => dest.Terms, opt => opt.MapFrom(src => src.Terms)).ReverseMap();
            CreateMap<AddAcademicYearCommand, AcademicYear>();
            CreateMap<UpdateAcademicYearCommand, AcademicYear>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
