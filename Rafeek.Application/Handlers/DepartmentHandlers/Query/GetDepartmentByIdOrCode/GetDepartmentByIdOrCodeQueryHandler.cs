using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetDepartmentByIdOrCode
{
    public class GetDepartmentByIdOrCodeQueryHandler : IRequestHandler<GetDepartmentByIdOrCodeQuery,DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetDepartmentByIdOrCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<Messages> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentByIdOrCodeQuery request, CancellationToken cancellationToken)
        {
            Guid.TryParse(request.IdOrCode, out Guid id);

            var department = await _unitOfWork.DepartmentRepository.GetAll(d => d.Id == id || d.Code == request.IdOrCode)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return departmentDto;
        }
    }
}
