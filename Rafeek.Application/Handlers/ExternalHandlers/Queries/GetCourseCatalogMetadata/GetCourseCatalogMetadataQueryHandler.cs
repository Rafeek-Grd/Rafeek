using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetCourseCatalogMetadata
{
    public class GetCourseCatalogMetadataQueryHandler : IRequestHandler<GetCourseCatalogMetadataQuery, PagginatedResult<CourseMetadataDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCourseCatalogMetadataQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagginatedResult<CourseMetadataDto>> Handle(GetCourseCatalogMetadataQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.CourseRepository
                .GetAll()
                .AsNoTracking()
                .Include(c => c.Department)
                .Select(c => new CourseMetadataDto
                {
                    Code = c.Code,
                    Title = c.Title,
                    Category = c.Department != null ? c.Department.Name : "General"
                });

            List<CourseMetadataDto> items;
            int totalCount;

            if (request.PageNumber == -1)
            {
                items = await query.ToListAsync(cancellationToken);
                totalCount = items.Count;
            }
            else
            {
                totalCount = await query.CountAsync(cancellationToken);
                items = await query
                    .OrderBy(c => c.Code)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }

            return new PagginatedResult<CourseMetadataDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
