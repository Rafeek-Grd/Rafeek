using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;
using Rafeek.Application.Common.Mappings;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllCoursesInDepartmentPagginated
{
    public class GetAllCoursesInDepartmentPagginatedQueryHandler : IRequestHandler<GetAllCoursesInDepartmentPagginatedQuery, PagginatedResult<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCoursesInDepartmentPagginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<CourseDto>> Handle(GetAllCoursesInDepartmentPagginatedQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CourseRepository.GetAll(c => c.DepartmentId == request.DepartmentId)
                .AsNoTracking()
                .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
