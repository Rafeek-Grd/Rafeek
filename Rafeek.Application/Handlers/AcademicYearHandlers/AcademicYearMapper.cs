using AutoMapper;
using Rafeek.Domain.Entities;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands;

namespace Rafeek.Application.Handlers.AcademicYearHandlers
{
    public class AcademicYearMapper : Profile
    {
        public AcademicYearMapper()
        {
            CreateMap<AcademicYear, AcademicYearDto>().ReverseMap();
            CreateMap<CreateAcademicYearCommand, AcademicYear>();
            CreateMap<UpdateAcademicYearCommand, AcademicYear>();
        }
    }
}
