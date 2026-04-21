using AutoMapper;
using Rafeek.Application.Common.Models.AI;
using Rafeek.Application.Handlers.CareerHandlers.DTOs;
using Rafeek.Application.Handlers.LearningResourceHandlers.DTOs;
using Rafeek.Application.Handlers.StudyPlanHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class AIProfile : Profile
    {
        public AIProfile()
        {
            CreateMap<AITimetableResponseDto, AITimetable>()
                .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.Stats.OptionName))
                .ForMember(dest => dest.MaxLoad, opt => opt.MapFrom(src => src.Stats.MaxLoad))
                .ForMember(dest => dest.TotalDays, opt => opt.MapFrom(src => src.Stats.TotalDays))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Schedule));

            CreateMap<ScheduledItemDto, AITimetableItem>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.SectionId))
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats));

            CreateMap<CareerSuggestion, CareerSuggestionDto>();
            CreateMap<StudyPlan, StudyPlanDto>();
            CreateMap<LearningResource, LearningResourceDto>();
        }
    }
}
