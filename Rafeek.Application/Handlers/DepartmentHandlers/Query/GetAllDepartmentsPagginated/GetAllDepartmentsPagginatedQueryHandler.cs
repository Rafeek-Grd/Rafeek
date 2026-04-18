using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Common.Mappings;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;
using Rafeek.Application.Common.Extensions;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllDepartmentsPagginated
{
    public class GetAllDepartmentsPagginatedQueryHandler : IRequestHandler<GetAllDepartmentsPagginatedQuery, PagginatedResult<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDepartmentsPagginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagginatedResult<DepartmentDto>> Handle(GetAllDepartmentsPagginatedQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DepartmentRepository.GetAll()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(request.Search), d => d.Name.Contains(request.Search!) || d.Code.Contains(request.Search!))
                .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
