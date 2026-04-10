using AutoMapper;
using Rafeek.Domain.Entities;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands;

namespace Rafeek.Application.Handlers.AcademicTermHandlers
{
    public class AcademicTermMapper : Profile
    {
        public AcademicTermMapper()
        {
            CreateMap<AcademicTerm, AcademicTermDto>().ReverseMap();
            CreateMap<CreateAcademicTermCommand, AcademicTerm>();
            CreateMap<UpdateAcademicTermCommand, AcademicTerm>();
        }
    }
}
