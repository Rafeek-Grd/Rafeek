using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorSections
{
    public class GetInstructorSectionsQueryHandlerPagginated : IRequestHandler<GetInstructorSectionsQueryPagginated, PagginatedResult<InstructorSectionDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;

        public GetInstructorSectionsQueryHandlerPagginated(IUnitOfWork ctx, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<InstructorSectionDto>> Handle(GetInstructorSectionsQueryPagginated request, CancellationToken cancellationToken)
        {
            var instructorUserId = _currentUserService.UserId;

            return await _ctx.SectionRepository
                .IncludeAll(null)
                .Where(x => x.Instructor.UserId == instructorUserId)
                .AsNoTracking()
                .Select(x => new InstructorSectionDto
                {
                    Id = x.Id,
                    CourseTitle = x.Course.Title,
                    CourseCode = x.Course.Code,
                    Day = x.Day,
                    Time = x.Time,
                    Capacity = x.Capacity,
                    EnrolledCount = x.Enrollments.Count
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
