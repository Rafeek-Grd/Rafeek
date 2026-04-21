using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetAllCoursesPaginated
{
    public class GetAllCoursesPaginatedQueryHandler 
        : IRequestHandler<GetAllCoursesPaginatedQuery, PagginatedResult<CourseDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAllCoursesPaginatedQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<CourseDto>> Handle(
            GetAllCoursesPaginatedQuery request, 
            CancellationToken cancellationToken)
        {
            var query = _ctx.CourseRepository.GetAll();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(x => x.Title.Contains(request.SearchTerm) || x.Code.Contains(request.SearchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderBy(x => x.Code)
                .PaginatedListAsync();

            var dtos = _mapper.Map<PagginatedResult<CourseDto>>(entities);

            return dtos;
        }
    }
}
