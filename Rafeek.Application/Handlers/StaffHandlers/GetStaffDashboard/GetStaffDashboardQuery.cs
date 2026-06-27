using MediatR;
using System;

namespace Rafeek.Application.Handlers.StaffHandlers.GetStaffDashboard
{
    public class GetStaffDashboardQuery : IRequest<GetStaffDashboardDto>
    {
        public string? AcademicStatus { get; set; }
        public string? SearchTerm { get; set; }
        public float? Cgpa { get; set; }
        public Guid? DepartmentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
