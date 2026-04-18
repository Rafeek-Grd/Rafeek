using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;
using Rafeek.Domain.Enums;
using Rafeek.Application.Common.Mappings;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllUsersInDepartmentPagginated
{
    public class GetAllUsersInDepartmentPagginatedQueryHandler : IRequestHandler<GetAllUsersInDepartmentPagginatedQuery, PagginatedResult<DepartmentUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly IRafeekDbContext _dbContext;

        public GetAllUsersInDepartmentPagginatedQueryHandler(IUnitOfWork unitOfWork, IIdentityUnitOfWork identityUnitOfWork, IRafeekDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _identityUnitOfWork = identityUnitOfWork;
            _dbContext = dbContext;
        }

        public async Task<PagginatedResult<DepartmentUserDto>> Handle(GetAllUsersInDepartmentPagginatedQuery request, CancellationToken cancellationToken)
        {
            var studentQuery = _unitOfWork.StudentRepository.GetAll(s => s.DepartmentId == request.DepartmentId)
                .Select(s => new { s.UserId, UniversityCode = s.UniversityCode, Role = UserType.Student.ToString() });

            var doctorQuery = _unitOfWork.DoctorRepository.GetAll(d => d.DepartmentId == request.DepartmentId)
                .Select(d => new { d.UserId, UniversityCode = d.EmployeeCode, Role = UserType.Doctor.ToString() });

            var instructorQuery = _unitOfWork.InstructorRepository.GetAll(i => i.DepartmentId == request.DepartmentId)
                .Select(i => new { i.UserId, UniversityCode = i.EmployeeCode, Role = UserType.Instructor.ToString() });

            var combinedQuery = studentQuery.Concat(doctorQuery).Concat(instructorQuery);

            var pagedItems = await combinedQuery.AsNoTracking().PaginatedListAsync(request.PageNumber, request.PageSize);

            if (pagedItems.TotalCount == 0)
                return PagginatedResult<DepartmentUserDto>.Create(new List<DepartmentUserDto>(), 0, request.PageNumber, request.PageSize);

            var userIds = pagedItems.Items.Select(u => u.UserId).ToList();
            var users = await _identityUnitOfWork.ApplicationUserRepository.GetAll(u => userIds.Contains(u.Id))
                .AsNoTracking()
                .ToDictionaryAsync(u => u.Id, u => u, cancellationToken);

            var resultDtos = pagedItems.Items.Select(item => {
                var user = users.GetValueOrDefault(item.UserId);
                return new DepartmentUserDto
                {
                    UserId = item.UserId,
                    FullName = user?.FullName!,
                    Email = user?.Email,
                    ProfilePictureUrl = user?.ProfilePictureUrl,
                    Role = item.Role,
                    UniversityCode = item.UniversityCode
                };
            }).ToList();

            return PagginatedResult<DepartmentUserDto>.Create(resultDtos, pagedItems.TotalCount, request.PageNumber, request.PageSize);
        }
    }
}
