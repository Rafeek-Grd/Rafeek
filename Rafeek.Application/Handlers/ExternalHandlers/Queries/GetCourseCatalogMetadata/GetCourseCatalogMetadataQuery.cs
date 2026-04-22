using MediatR;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetCourseCatalogMetadata
{
    public class GetCourseCatalogMetadataQuery : IRequest<List<CourseMetadataDto>>
    {
    }
}
