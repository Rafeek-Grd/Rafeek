using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CareerHandlers.DTOs;

namespace Rafeek.Application.Handlers.CareerHandlers.Queries.GetCareerSuggestionsByStudent
{
    public class GetCareerSuggestionsByStudentQuery : IRequest<PagginatedResult<CareerSuggestionDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public Guid StudentId { get; set; }
    }
}
