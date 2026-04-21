using AutoMapper;
using Rafeek.Application.Handlers.GPAHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class GPASimulatorProfile : Profile
    {
        public GPASimulatorProfile()
        {
            CreateMap<GPASimulatorLog, GPASimulatorLogDto>();
        }
    }
}
