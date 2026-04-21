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

            CreateMap<Course, CourseDetailDto>()
                .ForMember(d => d.CourseId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.EnrolledStudents, opt => opt.MapFrom(s => s.Enrollments.Count))
                .ForMember(d => d.Capacity, opt => opt.MapFrom(s => s.Enrollments.Select(e => e.Section.Capacity).FirstOrDefault()))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(s => s.Enrollments.SelectMany(e => e.Section.CalendarEvents).OrderBy(ev => ev.EventDate).Select(ev => (DateTime?)ev.EventDate).FirstOrDefault()))
                .ForMember(d => d.RegistrationOpenDate, opt => opt.MapFrom(s => s.Enrollments.SelectMany(e => e.Section.CalendarEvents).OrderBy(ev => ev.EventDate).Select(ev => (DateTime?)ev.EventDate).FirstOrDefault()))
                .ForMember(d => d.AcademicTerm, opt => opt.MapFrom(s => s.Enrollments.SelectMany(e => e.Section.CalendarEvents).Where(ev => ev.AcademicTerm != null).Select(ev => ev.AcademicTerm!.Name).FirstOrDefault()))
                .ForMember(d => d.IsTheoretical, opt => opt.MapFrom(s => true))
                .ForMember(d => d.IsPractical, opt => opt.MapFrom(s => false))
                .ForMember(d => d.TargetLevel, opt => opt.MapFrom(s => 3))
                .ForMember(d => d.Instructors, opt => opt.MapFrom(s => s.Enrollments.Select(e => e.Section.Instructor).Distinct().Select(i => new CourseInstructorDto
                {
                    InstructorId = i.Id,
                    FullName = i.User.FullName,
                    Email = i.User.Email!,
                    ProfilePictureUrl = i.User.ProfilePictureUrl
                })))
                .ForMember(d => d.Prerequisites, opt => opt.MapFrom(s => s.Prerequisites.Select(p => new PrerequisiteStatusDto
                {
                    CourseId = p.PrerequisiteId,
                    Code = p.Prerequisite.Code,
                    Title = p.Prerequisite.Title
                })))
                .ForMember(d => d.RegistrationStatus, opt => opt.Ignore())
                .ForMember(d => d.RegistrationStatusLabel, opt => opt.Ignore())
                .ForMember(d => d.StudyPlanDistribution, opt => opt.Ignore())
                .ForMember(d => d.Notifications, opt => opt.Ignore());
        }
    }
}
