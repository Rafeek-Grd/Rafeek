using AutoMapper;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Domain.Entities;
using System.Linq;

namespace Rafeek.Application.Handlers.StudentHandlers.Mappings
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<Student, StudentProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UniversityCode, opt => opt.MapFrom(src => src.UniversityCode))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                .ForMember(dest => dest.CurrentGPA, opt => opt.MapFrom(src => src.AcademicProfile != null ? src.AcademicProfile.GPA : 0))
                .ForMember(dest => dest.CumulativeGPA, opt => opt.MapFrom(src => src.AcademicProfile != null ? src.AcademicProfile.CGPA : 0))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.CompletedHours, opt => opt.MapFrom(src => src.AcademicProfile != null ? src.AcademicProfile.CompletedCredits : 0))
                .ForMember(dest => dest.AcademicAdvisorName, opt => opt.MapFrom(src => src.AcademicAdvisor != null && src.AcademicAdvisor.User != null ? src.AcademicAdvisor.User.FullName : null))
                .ForMember(dest => dest.AcademicHistory, opt => opt.Ignore()); 

            CreateMap<Enrollment, CourseGradeDto>()
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.Code))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.CreditHours, opt => opt.MapFrom(src => src.Course.CreditHours))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Grades.OrderByDescending(g => g.CreatedAt).Select(g => g.GradeValue).FirstOrDefault()));
        }
    }
}
