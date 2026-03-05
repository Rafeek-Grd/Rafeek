using AutoMapper;
using Rafeek.Domain.Entities;
using Rafeek.Application.Handlers.AuthHandlers.Query;

namespace Rafeek.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, GetUserProfileQueryResponse>();
        }
    }
}
