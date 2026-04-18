using AutoMapper;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
        }
    }
}
