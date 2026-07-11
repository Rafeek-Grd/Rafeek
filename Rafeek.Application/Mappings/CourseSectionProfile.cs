using AutoMapper;
using Rafeek.Application.Handlers.CourseSectionHandlers.Commands.CreateCourseSection;
using Rafeek.Application.Handlers.CourseSectionHandlers.Commands.UpdateCourseSection;
using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class CourseSectionProfile : Profile
    {
        public CourseSectionProfile()
        {
            CreateMap<CreateCourseSectionCommand, CourseSection>();
            CreateMap<UpdateCourseSectionCommand, CourseSection>();
            CreateMap<CourseSection, CourseSectionDto>();
        }
    }
}
