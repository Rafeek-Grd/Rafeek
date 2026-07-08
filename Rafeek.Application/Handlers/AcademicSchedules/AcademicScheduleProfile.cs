using AutoMapper;
using Rafeek.Application.Handlers.AcademicSchedules.Commands.CreateAcadmicSchedule;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AcademicSchedules
{
    public class AcademicScheduleProfile: Profile
    {
        public AcademicScheduleProfile()
        {
            CreateMap<CreateAcadmicScheduleCommand, LectureGroup>().ReverseMap()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
