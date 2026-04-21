using AutoMapper;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<Section, SectionDto>();
        }
    }
}
