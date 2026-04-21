using AutoMapper;
using Rafeek.Application.Handlers.ReminderHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class ReminderProfile : Profile
    {
        public ReminderProfile()
        {
            CreateMap<Rafeek.Application.Handlers.ReminderHandlers.Commands.CreateReminder.CreateReminderCommand, Reminder>();
            CreateMap<Reminder, ReminderDto>();
        }
    }
}
