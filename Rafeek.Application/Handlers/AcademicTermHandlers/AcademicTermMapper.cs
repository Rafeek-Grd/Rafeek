using AutoMapper;
using Rafeek.Domain.Entities;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands.CreateAcademicTerm;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands.UpdateAcademicTerm;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicTermHandlers
{
    public class AcademicTermMapper : Profile
    {
        public AcademicTermMapper()
        {
            CreateMap<AcademicTerm, AcademicTermDto>()
                .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src => src.AcademicYear))
                .ReverseMap();

            CreateMap<CreateAcademicTermCommand, AcademicTerm>();
            CreateMap<UpdateAcademicTermCommand, AcademicTerm>()
                .ForMember(dest => dest.TermType, opt => opt.MapFrom(src => src.TermType!.Value))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate!.Value))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate!.Value))
                .ForMember(dest => dest.RegistrationStartDate, opt => opt.MapFrom(src => src.RegistrationStartDate))
                .ForMember(dest => dest.RegistrationEndDate, opt => opt.MapFrom(src => src.RegistrationEndDate))
                .ForMember(dest => dest.DropDeadline, opt => opt.MapFrom(src => src.DropDeadline))
                .ForMember(dest => dest.ExamStartDate, opt => opt.MapFrom(src => src.ExamStartDate))
                .ForMember(dest => dest.ExamEndDate, opt => opt.MapFrom(src => src.ExamEndDate))
                .ForMember(dest => dest.AcademicYearId, opt => opt.MapFrom(src => src.AcademicYearId!.Value));
        }
    }
}
