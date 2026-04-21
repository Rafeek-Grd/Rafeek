using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.LearningResourceHandlers.DTOs;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.LearningResourceHandlers.Queries.GetAllLearningResources
{
    public class GetAllLearningResourcesQueryPagginated : IRequest<PagginatedResult<LearningResourceDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public ResourceType? ResourceType { get; set; }
    }
}
