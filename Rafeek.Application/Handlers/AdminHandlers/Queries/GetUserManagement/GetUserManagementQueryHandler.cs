using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetUserManagement
{
    public class GetUserManagementQueryHandler : IRequestHandler<GetUserManagementQuery, PagginatedResult<UserManagementListItemDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetUserManagementQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<PagginatedResult<UserManagementListItemDto>> Handle(GetUserManagementQuery request, CancellationToken cancellationToken)
        {
            if (request.TabRole == UserType.Student)
            {
                var query = _context.Students
                    .AsNoTracking()
                    .Include(s => s.User)
                    .Include(s => s.Department)
                    .Where(s => s.User.UserTypes.HasFlag(UserType.Student))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var term = request.SearchTerm.Trim();
                    query = query.Where(x => x.User.FullName.Contains(term) || x.User.Email!.Contains(term) || x.UniversityCode.Contains(term));
                }

                if (request.DepartmentId.HasValue)
                    query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    bool isActive = request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                    query = query.Where(x => x.User.IsUniversityEmailActivated == isActive);
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var projectedQuery = query
                    .OrderBy(x => x.User.FullName)
                    .Select(x => new UserManagementListItemDto
                    {
                        UserId = x.UserId,
                        FullName = x.User.FullName,
                        Email = x.User.Email!,
                        Role = "Student",
                        DepartmentName = x.Department != null ? x.Department.Name : null,
                        IdentificationNumber = x.UniversityCode,
                        Status = x.User.IsUniversityEmailActivated ? "Active" : "Inactive",
                        StatusLabel = x.User.IsUniversityEmailActivated ? "نشط" : "غير نشط"
                    });

                if (request.PageNumber != -1)
                {
                    projectedQuery = projectedQuery
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize);
                }

                var items = await projectedQuery.ToListAsync(cancellationToken);
                return PagginatedResult<UserManagementListItemDto>.Create(items.AsReadOnly(), totalCount, request.PageNumber, request.PageSize);
            }

            if (request.TabRole == UserType.Professor)
            {
                var query = _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.User)
                    .Include(d => d.Department)
                    .Where(d => d.User.UserTypes.HasFlag(UserType.Professor))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var term = request.SearchTerm.Trim();
                    query = query.Where(x => x.User.FullName.Contains(term) || x.User.Email!.Contains(term) || (x.EmployeeCode != null && x.EmployeeCode.Contains(term)));
                }

                if (request.DepartmentId.HasValue)
                    query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    bool isActive = request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                    query = query.Where(x => x.User.IsUniversityEmailActivated == isActive);
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var projectedQuery = query
                    .OrderBy(x => x.User.FullName)
                    .Select(x => new UserManagementListItemDto
                    {
                        UserId = x.UserId,
                        FullName = x.User.FullName,
                        Email = x.User.Email!,
                        Role = "Professor",
                        DepartmentName = x.Department != null ? x.Department.Name : null,
                        IdentificationNumber = x.EmployeeCode ?? "-",
                        Status = x.User.IsUniversityEmailActivated ? "Active" : "Inactive",
                        StatusLabel = x.User.IsUniversityEmailActivated ? "نشط" : "غير نشط"
                    });

                if (request.PageNumber != -1)
                {
                    projectedQuery = projectedQuery
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize);
                }

                var items = await projectedQuery.ToListAsync(cancellationToken);
                return PagginatedResult<UserManagementListItemDto>.Create(items.AsReadOnly(), totalCount, request.PageNumber, request.PageSize);
            }

            if (request.TabRole == UserType.Mentor)
            {
                var query = _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.User)
                    .Include(d => d.Department)
                    .Where(d => d.User.UserTypes.HasFlag(UserType.Mentor))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var term = request.SearchTerm.Trim();
                    query = query.Where(x => x.User.FullName.Contains(term) || x.User.Email!.Contains(term) || (x.EmployeeCode != null && x.EmployeeCode.Contains(term)));
                }

                if (request.DepartmentId.HasValue)
                    query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    bool isActive = request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                    query = query.Where(x => x.User.IsUniversityEmailActivated == isActive);
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var projectedQuery = query
                    .OrderBy(x => x.User.FullName)
                    .Select(x => new UserManagementListItemDto
                    {
                        UserId = x.UserId,
                        FullName = x.User.FullName,
                        Email = x.User.Email!,
                        Role = "Mentor",
                        DepartmentName = x.Department != null ? x.Department.Name : null,
                        IdentificationNumber = x.EmployeeCode ?? "-",
                        Status = x.User.IsUniversityEmailActivated ? "Active" : "Inactive",
                        StatusLabel = x.User.IsUniversityEmailActivated ? "نشط" : "غير نشط"
                    });

                if (request.PageNumber != -1)
                {
                    projectedQuery = projectedQuery
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize);
                }

                var items = await projectedQuery.ToListAsync(cancellationToken);
                return PagginatedResult<UserManagementListItemDto>.Create(items.AsReadOnly(), totalCount, request.PageNumber, request.PageSize);
            }

            // Staff
            {
                var query = _context.Staffs
                    .AsNoTracking()
                    .Include(s => s.User)
                    .Where(s => s.User.UserTypes.HasFlag(UserType.Staff))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var term = request.SearchTerm.Trim();
                    query = query.Where(x => x.User.FullName.Contains(term) || x.User.Email!.Contains(term) || (x.EmployeeCode != null && x.EmployeeCode.Contains(term)));
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    bool isActive = request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                    query = query.Where(x => x.User.IsUniversityEmailActivated == isActive);
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var projectedQuery = query
                    .OrderBy(x => x.User.FullName)
                    .Select(x => new UserManagementListItemDto
                    {
                        UserId = x.UserId,
                        FullName = x.User.FullName,
                        Email = x.User.Email!,
                        Role = "Staff",
                        DepartmentName = null,
                        IdentificationNumber = x.EmployeeCode ?? "-",
                        Status = x.User.IsUniversityEmailActivated ? "Active" : "Inactive",
                        StatusLabel = x.User.IsUniversityEmailActivated ? "نشط" : "غير نشط"
                    });

                if (request.PageNumber != -1)
                {
                    projectedQuery = projectedQuery
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize);
                }

                var items = await projectedQuery.ToListAsync(cancellationToken);
                return PagginatedResult<UserManagementListItemDto>.Create(items.AsReadOnly(), totalCount, request.PageNumber, request.PageSize);
            }
        }
    }
}
