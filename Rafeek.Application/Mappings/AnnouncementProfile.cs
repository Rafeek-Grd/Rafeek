using AutoMapper;
using Rafeek.Application.Handlers.AnnouncementHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class AnnouncementProfile : Profile
    {
        public AnnouncementProfile()
        {
            CreateMap<Announcement, AnnouncementDto>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.AudienceTypeLabel, opt => opt.MapFrom(s => s.AudienceType == 0 ? "جميع الطلاب" : s.AudienceType == 1 ? "أقسام محددة" : "حسب المستوى الأكاديمي"))
                .ForMember(d => d.StatusLabel, opt => opt.MapFrom(s => s.IsDeactivated ? "ملغي التنشيط" : s.IsSent ? "تم الإرسال" : "مجدول"));
        }
    }
}
