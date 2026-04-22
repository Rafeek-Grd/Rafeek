using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetCourseCatalogMetadata
{
    public class GetCourseCatalogMetadataQueryHandler : IRequestHandler<GetCourseCatalogMetadataQuery, List<CourseMetadataDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCourseCatalogMetadataQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CourseMetadataDto>> Handle(GetCourseCatalogMetadataQuery request, CancellationToken cancellationToken)
        {
            var courses = await _unitOfWork.CourseRepository
                .GetAll()
                .AsNoTracking()
                .Include(c => c.Department)
                .ToListAsync(cancellationToken);

            return courses.Select(c => new CourseMetadataDto
            {
                Code = c.Code,
                Title = c.Title,
                Category = c.Department?.Name ?? "General"
            }).ToList();
        }
    }
}
