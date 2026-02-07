using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers
{
    public class SignMapper: Profile
    {
        public SignMapper()
        {
            CreateMap<AuthResult, RefreshToken>()
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpiresIn))
            .ForMember(dest => dest.JwtId, opt => opt.MapFrom(src => src.Token));

            CreateMap<IdentityUser<Guid>, SignResponse>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<AuthResult, SignResponse>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(dest => dest.TokenType, opt => opt.MapFrom(src => src.TokenType))
                .ForMember(dest => dest.ExpiresIn, opt => opt.MapFrom(src => src.ExpiresIn));
        }
    }
}
