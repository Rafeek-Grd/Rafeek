using MediatR;
using Rafeek.Application.Common.Models.AI;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetCourseCatalogMetadata
{
    public class GetCourseCatalogMetadataQuery : IRequest<List<CourseMetadataDto>>
    {
    }
}
