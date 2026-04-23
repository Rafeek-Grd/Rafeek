using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentsInSection
{
    public class GetStudentsInSectionQueryHandlerPagginated : IRequestHandler<GetStudentsInSectionQueryPagginated, PagginatedResult<SectionStudentDto>>
    {
        private readonly IUnitOfWork _ctx;

        public GetStudentsInSectionQueryHandlerPagginated(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagginatedResult<SectionStudentDto>> Handle(GetStudentsInSectionQueryPagginated request, CancellationToken cancellationToken)
        {
            return await _ctx.EnrollmentRepository
                .IncludeAll(e => e.SectionId == request.SectionId)
                .AsNoTracking()
                .Select(e => new SectionStudentDto
                {
                    StudentId = e.StudentId,
                    FullName = e.Student.User.FullName,
                    UniversityCode = e.Student.UniversityCode,
                    ProfileImageUrl = e.Student.User.ProfilePictureUrl,
                    EnrollmentStatus = e.Status
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
