using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Extensions;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AdvisorHandlers.DTOs;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Queries.GetAllGuidenceSupportRequests
{
    public class GetAllGuidenceSupportRequestsPagginatedQueryHandler : IRequestHandler<GetAllGuidenceSupportRequestsPagginatedQuery, PagginatedResult<GuidanceRequestDto>>
    {
        private readonly IUnitOfWork _ctx;

        public GetAllGuidenceSupportRequestsPagginatedQueryHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagginatedResult<GuidanceRequestDto>> Handle(GetAllGuidenceSupportRequestsPagginatedQuery request, CancellationToken cancellationToken)
        {
            return await _ctx.StudentSupportRepository
                .IncludeAll(null)
                .AsNoTracking()
                .WhereIf(request.RequestStatus.HasValue, x => x.StudentSupportStatus == request.RequestStatus)
                .WhereIf(!string.IsNullOrWhiteSpace(request.SearchTerm), x => x.Student.User.FullName.Contains(request.SearchTerm!) | x.Student.User.Email!.Contains(request.SearchTerm!))
                .WhereIf(request.DepartmentId.HasValue, x => x.Student.DepartmentId == request.DepartmentId)
                .WhereIf(request.AdvisorId.HasValue, x => x.Student.AcademicAdvisorId == request.AdvisorId)
                .WhereIf(request.StudentId.HasValue, x => x.StudentId == request.StudentId)
                 .Select(x => new GuidanceRequestDto
                 {
                     Id = x.Id,
                     Title = x.Title,
                     Description = x.Description,
                     StudentSupportStatus = x.StudentSupportStatus,
                     StudentId = x.StudentId,
                     Student = new StudentDto
                     {
                         FullName = x.Student.User.FullName,
                         Email = x.Student.User.Email!,
                         UniversityCode = x.Student.UniversityCode,
                         Department = x.Student.Department == null ? null : new DepartmentDto
                         {
                             Id = x.Student.Department.Id,
                             Name = x.Student.Department.Name
                         },
                         Status = x.Student.Status,
                         Level = x.Student.Level,
                         Term = x.Student.Term,
                         AcademicProfileDto = new AcademicProfileDto
                         {
                             AcademicProfileId = x.Student.AcademicProfileId,
                             GPA = x.Student.AcademicProfile!.GPA,
                             CGPA = x.Student.AcademicProfile.CGPA,
                             CompletedCredits = x.Student.AcademicProfile.CompletedCredits,
                             RemainingCredits = x.Student.AcademicProfile.RemainingCredits
                         },
                         Advisor = x.Student.AcademicAdvisor == null ? null : new AdvisorDto
                         {
                             Id = x.Student.AcademicAdvisor.Id,
                             UserId = x.Student.AcademicAdvisor.UserId,
                             AdvisorName = x.Student.AcademicAdvisor.User.FullName,
                             Email = x.Student.AcademicAdvisor.User.Email!,
                             EmployeeCode = x.Student.AcademicAdvisor.EmployeeCode,
                             Department = x.Student.AcademicAdvisor.Department == null ? null : new DepartmentDto
                             {
                                 Id = x.Student.AcademicAdvisor.Department.Id,
                                 Name = x.Student.AcademicAdvisor.Department.Name
                             }
                         }
                     },
                     CreatedAt = x.CreatedAt,
                     UpdatedAt = x.UpdatedAt,
                 })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
