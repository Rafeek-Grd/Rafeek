using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Extensions;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AdvisorHandlers.DTOs;
using Rafeek.Application.Handlers.DocumentHandlers.DTOs;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.DocumentHandlers.Queries.GetDocumentRequestsPaginated
{
    public class GetDocumentRequestsPaginatedQueryHandler : IRequestHandler<GetDocumentRequestsPaginatedQuery, PagginatedResult<DocumentRequestDto>>
    {
        private readonly IUnitOfWork _ctx;

        public GetDocumentRequestsPaginatedQueryHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagginatedResult<DocumentRequestDto>> Handle(GetDocumentRequestsPaginatedQuery request, CancellationToken cancellationToken)
        {
            var query = _ctx.DocumentRequestRepository
                .IncludeAll(null)
                .AsNoTracking();

            // Apply filters
            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.DocumentType))
            {
                query = query.Where(x => x.DocumentType == request.DocumentType);
            }

            if (request.StudentId.HasValue)
            {
                query = query.Where(x => x.StudentId == request.StudentId.Value);
            }

            if (request.AdvisorId.HasValue)
            {
                query = query.Where(x => x.Student.AcademicAdvisorId == request.AdvisorId.Value);
            }

            if (request.DepartmentId.HasValue)
            {
                query = query.Where(x => x.Student.DepartmentId == request.DepartmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                query = query.Where(x => x.Student.User.FullName.Contains(term) || x.Student.UniversityCode.Contains(term));
            }

            return await query
                .Select(x => new DocumentRequestDto
                {
                    Id = x.Id,
                    DocumentType = x.DocumentType,
                    Status = x.Status.ToString(),
                    Remarks = x.Remarks,
                    Topic = x.Topic,
                    AttachmentUrl = x.AttachmentUrl,
                    StudentId = x.StudentId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
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
                        AcademicProfileDto = x.Student.AcademicProfile == null ? null : new AcademicProfileDto
                        {
                            AcademicProfileId = x.Student.AcademicProfileId,
                            GPA = x.Student.AcademicProfile.GPA,
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
                    }
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
