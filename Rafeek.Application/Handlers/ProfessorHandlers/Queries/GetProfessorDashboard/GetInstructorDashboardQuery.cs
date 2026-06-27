using MediatR;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using System;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQuery : IRequest<InstructorDashboardDto>
    {
        public Guid? LectureGroupId { get; set; }
        public string? AcademicStatus { get; set; }
        public string? SearchTerm { get; set; }
        public float? Cgpa { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
