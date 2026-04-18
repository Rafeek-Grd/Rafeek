using MediatR;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using System;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentDashboard
{
    public class GetStudentDashboardQuery : IRequest<StudentDashboardDto>
    {
        public Guid UserId { get; set; }
    }
}
