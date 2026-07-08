using AutoMapper;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Application.Handlers.CourseHandlers.Commands.CreateNewCourse;
using Rafeek.Application.Handlers.CourseHandlers.Commands.UpdateCourse;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateNewCourseCommand, Course>();
            CreateMap<UpdateCourseCommand, Course>();
            CreateMap<Course, CourseDto>();
            CreateMap<LectureGroup, LectureGroupDto>()
                .ForMember(d => d.DoctorName, opt => opt.MapFrom(s => s.Doctor != null ? s.Doctor.User.FullName : null))
                .ForMember(d => d.DoctorEmail, opt => opt.MapFrom(s => s.Doctor != null ? s.Doctor.User.Email : null))
                .ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location))
                .ForMember(d => d.EnrolledStudentsCount, opt => opt.MapFrom(s => s.Enrollments.Count));

            CreateMap<Course, CourseDetailDto>()
                .ForMember(d => d.CourseId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.EnrolledStudents, opt => opt.MapFrom(s => s.Enrollments.Count))
                .ForMember(d => d.Capacity, opt => opt.MapFrom(s => s.LectureGroups.Sum(lg => lg.Capacity)))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(s => s.RegistrationPeriods.Select(p => (DateTime?)p.AcademicTerm.StartDate).FirstOrDefault()))
                .ForMember(d => d.RegistrationOpenDate, opt => opt.MapFrom(s => s.RegistrationPeriods.Select(p => (DateTime?)p.RegistrationOpeningDate).FirstOrDefault()))
                .ForMember(d => d.RegistrationCloseDate, opt => opt.MapFrom(s => s.RegistrationPeriods.Select(p => (DateTime?)p.RegistrationClosingDate).FirstOrDefault()))
                .ForMember(d => d.AcademicTerm, opt => opt.MapFrom(s => s.RegistrationPeriods.Select(p => p.AcademicTerm.Name).FirstOrDefault()))
                .ForMember(d => d.IsTheoretical, opt => opt.MapFrom(s => true))
                .ForMember(d => d.IsPractical, opt => opt.MapFrom(s => false))
                .ForMember(d => d.TargetLevel, opt => opt.MapFrom(s => 3))
                .ForMember(d => d.WeeklyHours, opt => opt.MapFrom(s => new WeeklyHoursDto
                {
                    LectureHours = s.WeeklyLectureHours,
                    LabHours = s.WeeklyLabHours
                }))
                .ForMember(d => d.GradeDistribution, opt => opt.MapFrom(s => new GradeDistributionDto
                {
                    MidtermPercent = s.MidtermPercent,
                    FinalPercent = s.FinalPercent,
                    ProjectPercent = s.ProjectPercent
                }))
                .ForMember(d => d.Instructors, opt => opt.MapFrom(s => s.LectureGroups.Select(lg => lg.Doctor).Where(d => d != null).Distinct().Select(i => new CourseInstructorDto
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
                .ForMember(d => d.LectureGroups, opt => opt.MapFrom(s => s.LectureGroups))
                .ForMember(d => d.RegistrationStatus, opt => opt.Ignore())
                .ForMember(d => d.RegistrationStatusLabel, opt => opt.Ignore())
                .ForMember(d => d.StudyPlanDistribution, opt => opt.Ignore())
                .ForMember(d => d.Notifications, opt => opt.Ignore());
        }
    }
}
