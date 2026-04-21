using AutoMapper;
using Rafeek.Application.Handlers.DocumentHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class DocumentRequestProfile : Profile
    {
        public DocumentRequestProfile()
        {
            CreateMap<DocumentRequest, DocumentRequestDto>();
        }
    }
}
