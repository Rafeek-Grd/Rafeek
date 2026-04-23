using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQueryHandler : IRequestHandler<GetInstructorDashboardQuery, InstructorDashboardDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;

        public GetInstructorDashboardQueryHandler(IUnitOfWork ctx, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
        }

        public async Task<InstructorDashboardDto> Handle(GetInstructorDashboardQuery request, CancellationToken cancellationToken)
        {
            var instructorUserId = _currentUserService.UserId;

            var sectionIds = await _ctx.SectionRepository
                .IncludeAll(null)
                .Where(x => x.Instructor.UserId == instructorUserId)
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var totalStudents = await _ctx.EnrollmentRepository
                .GetAll(e => sectionIds.Contains(e.SectionId))
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync(cancellationToken);

            var pendingToGrade = await _ctx.AssignmentSubmissionRepository
                .IncludeAll(null)
                .Where(s => sectionIds.Contains(s.Assignment.SectionId) && s.Score == null)
                .CountAsync(cancellationToken);

            return new InstructorDashboardDto
            {
                TotalSections = sectionIds.Count,
                TotalStudents = totalStudents,
                PendingAssignmentsToGrade = pendingToGrade
            };
        }
    }
}
