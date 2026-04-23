using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Mappings;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorExamSchedule
{
    public class GetInstructorExamScheduleQueryHandlerPagginated : IRequestHandler<GetInstructorExamScheduleQueryPagginated, PagginatedResult<InstructorExamScheduleDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;

        public GetInstructorExamScheduleQueryHandlerPagginated(IUnitOfWork ctx, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<InstructorExamScheduleDto>> Handle(GetInstructorExamScheduleQueryPagginated request, CancellationToken cancellationToken)
        {
            var instructorUserId = _currentUserService.UserId;

            var sectionIds = await _ctx.SectionRepository
                .IncludeAll(null)
                .Where(x => x.Instructor.UserId == instructorUserId)
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            return await _ctx.AcademicCalendarRepository
                .IncludeAll(e => e.EventType == AcademicCalendarEventType.Exam
                              && e.SectionId.HasValue
                              && sectionIds.Contains(e.SectionId.Value))
                .AsNoTracking()
                .Select(e => new InstructorExamScheduleDto
                {
                    CourseTitle = e.Course != null ? e.Course.Title : string.Empty,
                    CourseCode = e.Course != null ? e.Course.Code : string.Empty,
                    ExamDate = e.EventDate,
                    Location = e.Location ?? string.Empty
                })
                .OrderBy(e => e.ExamDate)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
