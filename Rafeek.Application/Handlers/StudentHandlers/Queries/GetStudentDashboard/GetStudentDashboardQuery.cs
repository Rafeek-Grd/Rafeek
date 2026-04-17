using MediatR;
using System;

namespace Rafeek.Application.Handlers.StudentHandlers.Queries.GetStudentDashboard
{
    public class GetStudentDashboardQuery : IRequest<StudentDashboardDto>
    {
        public Guid UserId { get; set; }

        public GetStudentDashboardQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
